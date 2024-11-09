using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoder : MonoBehaviour
{
    public static LevelLoder Instance;

    public Animator transition;
    public float fadeDuration = 1f; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void TransitionToScene(int num)
    {
         StartCoroutine(PerformTransition(num));
    }

    // 实现场景过渡的协程
    private IEnumerator PerformTransition(int num)
    {
        transition.SetBool("start", true);
        transition.SetBool("end", false);

        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation async =  SceneManager.LoadSceneAsync(num);
        async.completed += OnLoadedScene;
    }

    private void OnLoadedScene(AsyncOperation operation)
    {
        transition.SetBool("start", false);
        transition.SetBool("end", true);
    }
}
