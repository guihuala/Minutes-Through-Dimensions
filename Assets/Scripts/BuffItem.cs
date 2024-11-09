using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffItem : MonoBehaviour
{
    public Buff buff; 
    private SpriteRenderer buffIconImage;
    private BuffInfo buffInfo;

    private void Awake()
    {
        buffInfo = transform.Find("trigger").GetComponent<BuffInfo>();
        buffIconImage = transform.Find("sprite").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 初始化 UI 显示 Buff 的信息
        if (buff != null)
        {
            if (buffIconImage != null)
                buffIconImage.sprite = buff.buffIcon;
            if(buffInfo != null)
            {
                buffInfo.buff = buff;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyBuffToPlayer(collision.gameObject);

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player)
            {
                player.isTransfer = true;
                player.PlayFadesAnim();
            }
        }
    }

    private void ApplyBuffToPlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && buff != null)
        {
            buff.Apply(playerController);
            playerController.PlayFadesAnim();
            GameManager.Instance.NextLevel();
        }
    }
}


