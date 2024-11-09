using UnityEngine;

public class BuffInfo : MonoBehaviour
{
    public Buff buff; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当玩家进入 InfoTrigger 区域时，显示 Buff 信息
        if (collision.CompareTag("Player") && buff != null)
        {
            UIManager.Instance.UpdateBuffInfo(buff.buffName, buff.description);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 当玩家离开 InfoTrigger 区域时，隐藏 Buff 信息
        if (collision.CompareTag("Player") && buff != null)
        {
            UIManager.Instance.HideBuffInfo();
        }
    }
}

