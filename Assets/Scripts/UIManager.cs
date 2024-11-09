using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // 单例模式

    private TextMeshProUGUI TimerText;
    private TextMeshProUGUI ScoreText;
    private TextMeshProUGUI LifeText;
    private TextMeshProUGUI BuffNameText; 
    private TextMeshProUGUI BuffDescriptionText;
    private TextMeshProUGUI SkillText;
    private TextMeshProUGUI gameOverText;

    public Image cooldownImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        TimerText = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        ScoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        LifeText = transform.Find("Life").GetComponent<TextMeshProUGUI>();
        BuffNameText = transform.Find("BuffName").GetComponent<TextMeshProUGUI>();
        BuffDescriptionText = transform.Find("BuffInfo").GetComponent<TextMeshProUGUI>();
        SkillText = transform.Find("Skill").GetComponent<TextMeshProUGUI>();

        gameOverText = transform.Find("gameover").GetComponent<TextMeshProUGUI>();

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnHealthChanged += UpdateLife;
            player.OnSkillTimeChanged += UpdateSkill;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnTimeChanged += UpdateTimer;
        }

    }

    private void Start()
    {
        UpdateInitialUI();
        HideBuffInfo();
        HideGameOver();
    }

    private void UpdateInitialUI()
    {
        if (GameManager.Instance != null)
        {
            UpdateTimer(GameManager.Instance.remainingTime);
            UpdateScore(GameManager.Instance.score); 
        }
    }

    public void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
    }

    private IEnumerator CooldownCoroutine(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (elapsed / duration);
            yield return null;
        }
        cooldownImage.fillAmount = 0;
    }

    public void UpdateTimer(float time)
    {
        if (TimerText != null)
        {
            TimerText.text = "Time: " + Mathf.CeilToInt(time).ToString(); 
        }
    }

    public void UpdateScore(int score)
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score: " + score.ToString();
        }
    }

    public void UpdateLife(int currentHealth, int maxHealth)
    {
        if (LifeText != null)
        {
            LifeText.text = currentHealth + "/" + maxHealth; 
        }
    }

    private void UpdateSkill(int currentSkillTimes, int maxSkillTimes)
    {
        if(SkillText != null)
        {
            SkillText.text = currentSkillTimes + "/" + maxSkillTimes;
        }
    }

    public void UpdateBuffInfo(string buffName, string buffDescription)
    {
        ShowBuffInfo();
        if (BuffNameText != null)
        {
            BuffNameText.text = buffName; 
        }
        if (BuffDescriptionText != null)
        {
            BuffDescriptionText.text = buffDescription;
        }
    }

    public void HideBuffInfo()
    {
        BuffNameText.gameObject.SetActive(false);
        BuffDescriptionText.gameObject.SetActive(false);
    }

    private void ShowBuffInfo()
    {
        BuffNameText.gameObject.SetActive(true);
        BuffDescriptionText.gameObject.SetActive(true);
    }

    private void HideGameOver()
    {
        gameOverText.gameObject.SetActive(false);
    }

    public void ShowGameOver(float second)
    {
        gameOverText.gameObject.SetActive(true);

        if(second <= 0) gameOverText.SetText("time up");
    }

    private void OnDestroy()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnHealthChanged -= UpdateLife;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnTimeChanged -= UpdateTimer;
        }
    }
}

