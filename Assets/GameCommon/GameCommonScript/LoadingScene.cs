using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingScene : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image progressBar;
    public TextMeshProUGUI loadingText;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    void Start()
    {
        StartCoroutine(LoadingSceneProcess());
    }

    IEnumerator LoadingSceneProcess()
    {
       AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        //생각보다 씬로딩속도가 빠를 수 있다. 그래서 페이크 로딩을 넣어줌
        //에셋번들로부터 리소쓰를 읽어와야할때

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            loadingText.text = "로딩 중(" + Mathf.RoundToInt(progressBar.fillAmount * 100).ToString() + "%)";

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }

            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;

                    yield break;
                }


            }
        }
    }
}
