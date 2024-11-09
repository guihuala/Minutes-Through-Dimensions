using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int highScore;
    private int highLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadScores(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadScores()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highLevel = PlayerPrefs.GetInt("HighLevel", 1); 
    }

    public void SaveScores(int currentScore, int currentLevel)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore; 
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (currentLevel > highLevel)
        {
            highLevel = currentLevel; 
            PlayerPrefs.SetInt("HighLevel", highLevel);
        }

        PlayerPrefs.Save(); 
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public int GetHighLevel()
    {
        return highLevel;
    }
}

