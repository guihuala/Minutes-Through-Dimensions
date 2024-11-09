using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float detectionRange = 3f; 

    private Vector3 randomDirection; 
    private float changeDirectionTime = 2f; 
    private float changeDirectionCounter;
    private Transform playerTransform; 
    private bool isChasingPlayer = false;

    [SerializeField]private GameObject particle;

    private void Start()
    {
        ChooseNewDirection();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);


            if (distanceToPlayer <= detectionRange)
            {
                isChasingPlayer = true; 
            }
            else
            {
                isChasingPlayer = false;
            }
        }

        if (isChasingPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            MoveEnemy();

            changeDirectionCounter -= Time.deltaTime;
            if (changeDirectionCounter <= 0)
            {
                ChooseNewDirection();
            }
        }
    }

    private void MoveEnemy()
    {
        transform.position += randomDirection * moveSpeed * Time.deltaTime;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.position += directionToPlayer * moveSpeed *1.2f * Time.deltaTime;
    }

    private void ChooseNewDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        changeDirectionCounter = changeDirectionTime;
    }

    public void die()
    {
        GameObject explosion = Instantiate(particle, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}


