using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static SceneType loadScene;      // 로드할 씬

    void Start()
    {
        StartCoroutine(Loading());
    }

    /// <summary>
    /// 로드할 씬을 로드하기 전에 로딩 씬으로 먼저 이동 후 로딩 연출을 함
    /// </summary>
    /// <param name="sceneType">로드할 씬</param>
    public static void LoadScene(SceneType sceneType)
    {
        loadScene = sceneType;
        SceneManager.LoadScene((int)SceneType.Loading);
    }

    IEnumerator Loading()
    {
        yield return null;

        AsyncOperation asyncOperation;
        asyncOperation = SceneManager.LoadSceneAsync((int)loadScene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
