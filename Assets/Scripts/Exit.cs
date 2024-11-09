using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player)
            {
                player.isTransfer = true;
                player.PlayFadesAnim();
            }

            GameManager.Instance.NextLevel();
        }
    }
}

