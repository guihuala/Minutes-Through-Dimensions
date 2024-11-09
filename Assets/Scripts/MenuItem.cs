using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItem : MonoBehaviour
{
    public string action;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Color green;
    [SerializeField] private Color gray;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (action == "sfx") ChangeSfxColor();
        else if (action == "music") ChangeBGMColor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (!player) return;

            if (action == "StartGame")
            {
                player.PlayFadesAnim();
                SfxManager.instance.PlaySFX(2);
                StartGame();
            }
            else if (action == "Tutorial")
            {
                player.PlayFadesAnim();
                SfxManager.instance.PlaySFX(2);
                OpenTutorial();
            }
            else if(action == "sfx")
            {
                SfxManager.instance.ToggleSFX();
                ChangeSfxColor();
            }
            else if(action == "music")
            {
                MusicManager.instance.ToggleMusic();
                ChangeBGMColor();
            }
            else if(action == "back")
            {
                player.PlayFadesAnim();
                SfxManager.instance.PlaySFX(2);
                BackToMenu();
            }
        }
    }

    private void StartGame()
    {
        LevelLoder.Instance.TransitionToScene(1);
    }

    private void BackToMenu()
    {
        LevelLoder.Instance.TransitionToScene(0);
    }

    private void OpenTutorial()
    {
        LevelLoder.Instance.TransitionToScene(2);
    }

    public void ChangeBGMColor()
    {
        if (MusicManager.instance.GetIsMusicOn())
        {
            spriteRenderer.color = green;
        }
        else
        {
            spriteRenderer.color = gray;
        }
    }

    public void ChangeSfxColor()
    {
        if (SfxManager.instance.GetIsSfxOn())
        {
            spriteRenderer.color = green;
        }
        else
        {
            spriteRenderer.color =gray;
        }
    }
}

