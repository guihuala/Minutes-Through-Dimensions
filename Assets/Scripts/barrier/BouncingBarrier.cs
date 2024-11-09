using UnityEngine;

public class BouncingBarrier : barrierController
{
    public float bounceDistance = 2f; 
    public float bounceDuration = 0.2f; 
    private bool isBouncing = false;

    public override void OnPlayerCollision(PlayerController player)
    {
        if (!isBouncing)
        {
            isBouncing = true;
            Vector2 bounceDirection = (player.transform.position - transform.position).normalized;
            StartCoroutine(BouncePlayer(player.transform, bounceDirection));
        }
    }

    private System.Collections.IEnumerator BouncePlayer(Transform playerTransform, Vector2 direction)
    {
        float elapsedTime = 0f;

        Vector3 startPosition = playerTransform.position;

        Vector3 targetPosition = startPosition + (Vector3)direction * bounceDistance;

        while (elapsedTime < bounceDuration)
        {
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / bounceDuration);
            elapsedTime += Time.deltaTime;

            yield return null; 
        }

        playerTransform.position = targetPosition;

        yield return new WaitForSeconds(0.5f);
        isBouncing = false;
    }
}


