using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // 移动属性
    public float moveSpeed = 7f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float detectionRadius = 5f;
    public float rotationSpeed = 5f;
    public bool isTransfer;
    private float dashTime;
    private Vector3 dashDirection;

    // 无敌属性
    public float invincibilityDuration = .3f;
    public bool isInvincible = false;

    // 生命属性
    private int healthMax = 3;
    private int health;
    public bool isDead = false;

    // 技能属性
    private int currentSkillTime;
    public int maxSkillTime = 3;
    private bool isCooling;

    // 回血属性
    private float healProbability = 0f; 
    public int healAmount = 1; 

    // 视觉效果
    private TrailRenderer trailRenderer;
    private Material material;
    private Animator transition;

    // 事件
    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnSkillTimeChanged;

    private void Start()
    {
        health = healthMax;
        NotifyHealthChanged();

        trailRenderer = GetComponentInChildren<TrailRenderer>();
        material = transform.GetComponent<SpriteRenderer>().material;
        transition = GetComponent<Animator>();

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged += Instance_OnLevelChanged; ;
        }

        currentSkillTime = maxSkillTime;
        NotifySkillTimeChanged();

        Invincibility();
    }

    private void Instance_OnLevelChanged()
    {
        Invincibility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentSkillTime > 0)
        {
            if (isDead || isTransfer) return;
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (dashTime > 0)
        {
            DashMove();
        }
        else
        {
            NormalMove();

            if (currentSkillTime < maxSkillTime && !isCooling)
            {
                if (UIManager.Instance)
                    UIManager.Instance.StartCooldown(dashCooldown);

                StartCoroutine(IncreaseSkillTimeAfterCooldown(dashCooldown));
            }
        }
    }

    private IEnumerator IncreaseSkillTimeAfterCooldown(float duration)
    {
        isCooling = true;
        yield return new WaitForSeconds(duration);
        if (currentSkillTime < maxSkillTime)
        {
            currentSkillTime++;
            NotifySkillTimeChanged();
        }
        isCooling = false;
    }

    private void NormalMove()
    {
        if (isDead || isTransfer) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f);
        transform.position += move * moveSpeed * Time.deltaTime;

        UpdateRotation(moveX, moveY);
    }

    private void DashMove()
    {
        transform.position += dashDirection * dashSpeed * Time.deltaTime;

        dashTime -= Time.deltaTime;

        if (dashTime <= 0)
        {
            trailRenderer.emitting = false;
        }
    }

    private void StartDash()
    {
        SfxManager.instance.PlaySFX(0);
        GameObject nearestEnemy = FindNearestEnemy();

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        if (nearestEnemy != null)
        {
            dashDirection = (nearestEnemy.transform.position - transform.position).normalized;
        }
        else
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (moveX == 0 && moveY == 0)
            {
                dashDirection = transform.up;
            }
            else
            {
                dashDirection = new Vector3(moveX, moveY, 0f).normalized;
            }
        }

        dashTime = dashDuration;
        StartCoroutine(ActivateInvincibility());
        UseSkill();
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= detectionRadius && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (!enemy) return;

            if (dashTime > 0)
            {
                enemy.die();
                SfxManager.instance.PlaySFX(3);
                GameManager.Instance.UpdateScore(5);

                AttemptHeal();
            }
            else
            {
                if (isInvincible) return;
                enemy.die();
                GetHurt();
            }
        }
    }

    private void AttemptHeal()
    {
        // 生成一个随机数以决定是否回血
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue < healProbability)
        {
            Heal(healAmount);
        }
    }

    private void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, healthMax);
        NotifyHealthChanged();
    }

    public void GetHurt()
    {
        if (isInvincible || isDead) return;

        health = Mathf.Clamp(health - 1, 0, healthMax);
        NotifyHealthChanged();

        // 画面震动
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        SfxManager.instance.PlaySFX(1);

        if (cameraController != null)
        {
            StartCoroutine(cameraController.CameraShake());
        }

        StartCoroutine(ActivateInvincibility());

        if (health == 0)
        {
            isDead = true;
            GameManager.Instance.GameOver();
        }
    }

    public void Invincibility()
    {
        StartCoroutine(ActivateInvincibility());
    }

    public void PlayFadesAnim()
    {
        transition.SetTrigger("fade");
        StartCoroutine(ActivateInvincibility());
    }

    // 无敌协程
    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // 无敌状态的视觉效果
        transition.SetTrigger("hurt");

        yield return new WaitForSeconds(invincibilityDuration);

        sprite.enabled = true;
        isInvincible = false;
    }

    public void IncreaseMaxHealth(int amount)
    {
        healthMax += amount;
        health += amount;
        NotifyHealthChanged();
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void AddSkillTime(int amount)
    {
        maxSkillTime += amount;
        NotifySkillTimeChanged();
    }

    public void ReduceCoolDownTime(float amount)
    {
        dashCooldown = dashCooldown * amount;
    }

    private void UseSkill()
    {
        currentSkillTime = Mathf.Clamp(currentSkillTime - 1, 0, maxSkillTime);
        NotifySkillTimeChanged();
    }

    private void NotifySkillTimeChanged()
    {
        OnSkillTimeChanged?.Invoke(currentSkillTime, maxSkillTime);
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(health, healthMax);
    }

    private void UpdateRotation(float moveX, float moveY)
    {
        float targetXRotation = 0f;
        float targetYRotation = 0f;

        if (moveY != 0)
        {
            targetXRotation = moveY > 0 ? -10f : 10f;
        }

        if (moveX != 0)
        {
            targetYRotation = moveX > 0 ? 10f : -10f;
        }

        Quaternion targetRotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // 方法用于增加回血概率
    public void IncreaseHealProbability(float amount)
    {
        healProbability = Mathf.Clamp(healProbability + amount, 0f, 1f); // 确保不超过 1
    }
}


