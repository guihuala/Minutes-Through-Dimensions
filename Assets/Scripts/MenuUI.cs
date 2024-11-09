using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    private TextMeshPro DateText;

    private void Awake()
    {
        DateText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        DateText.SetText("high score:" + ScoreManager.Instance.GetHighScore() + " high level:" + ScoreManager.Instance.GetHighLevel());
    }
}
