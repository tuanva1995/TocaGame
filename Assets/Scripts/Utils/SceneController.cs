using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Pixelplacement;

public class SceneController : Singleton<SceneController>
{
    private bool canLoadScene = true;
    // private SceneInstance currentScene;
    protected override void OnRegistration()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
#if !UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(_LoadSceneNoAddressable(sceneName));
    }
    private IEnumerator _LoadSceneNoAddressable(string sceneName)//use this function to load between a normal scene
    {
        var delay = new WaitForSecondsRealtime(0.1f);
        while (!canLoadScene)
        {
            yield return delay;
        }
        canLoadScene = false;
        string currentSceneName = SceneManager.GetActiveScene().name;
        var loadingAsyncTask = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        while (!loadingAsyncTask.isDone)
            yield return delay;
        var loadController = FindObjectOfType<LoadSceneController>();
        yield return new WaitForSecondsRealtime(0.4f);
        var asyncTask = SceneManager.UnloadSceneAsync(currentSceneName);
        asyncTask.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(0.1f);
        float progress = 0;
        while (!asyncTask.isDone && progress < 0.5f)
        {
            yield return delay;
            progress = Mathf.Min(progress + 0.06f, asyncTask.progress / 2f);
            loadController.UpdateLoadingBar(progress);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        asyncTask = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncTask.allowSceneActivation = false;
        bool flag = false;
        yield return new WaitForSecondsRealtime(0.1f);
        do
        {
            loadController.UpdateLoadingBar(0.5f + asyncTask.progress / 2);
            yield return delay;
            if (asyncTask.progress >= 0.9f && !flag) 
            {
                flag = true;
                asyncTask.allowSceneActivation = true;
            }
        }
        while (!asyncTask.isDone);
        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        yield return new WaitForSecondsRealtime(0.1f);
        loadController.FadeOut();
        yield return new WaitForSecondsRealtime(0.5f);
        asyncTask = SceneManager.UnloadSceneAsync("Loading");
        do
        {
            yield return delay;
        }
        while (!asyncTask.isDone);
        canLoadScene = true;
    }
}
