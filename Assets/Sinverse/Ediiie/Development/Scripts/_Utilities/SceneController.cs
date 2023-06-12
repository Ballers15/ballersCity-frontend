using CogniHab.Utils.Data;
using DG.Tweening;
using Ediiie.Audio;
using Sinverse;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController m_Instance;
    public static Action OnNewLevelLoaded;
    public static Action<string> OnUnloadSceneCompleted;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private LoadingProgressView loadingView;
    [SerializeField] private LoadingTextFactory loadingTextFactory;

    public static string CurrentScene;

    //public void ShowLoadings()
    //{
    //    loadingCanvas.SetActive(true);
    //}

    private void Awake()
    {
        m_Instance = this;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        //PlayerData.Create();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    public static void RestartScene()
    {
        //GameManager.IsGameOn = false;
        DOTween.KillAll(false);
        //GameManager.Reset();
    }

    public static void ReloadCurrentScene()
    {
        SceneProperties sceneProperties = new SceneProperties() { 
            isAsync = true, 
            loadSceneMode = LoadSceneMode.Single, 
            sceneName = CurrentScene, 
            showLoading = true 
        };
        Debug.Log("Check>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"+sceneProperties.sceneName);
        LoadScene(sceneProperties);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(scene);
        AudioManager.PlayAudio(scene.name);
        //OnNewLevelLoaded?.Invoke(scene.name);
        OnNewLevelLoaded?.Invoke();
        CurrentScene = scene.name;
    }

    public static void LoadScene(SceneProperties sceneProperties, Action callback = null)
    {
        string sceneName = sceneProperties.sceneName;

        Debug.LogWarning("load scene ============" + sceneName);

        if (sceneProperties.isAsync)
        {
            AudioManager.OnStopAudio?.Invoke();
            m_Instance.StartCoroutine(m_Instance.LoadSceneAsync(sceneName, sceneProperties, callback));
        }
        else
        {
            SceneManager.LoadScene(sceneName, sceneProperties.loadSceneMode);
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, SceneProperties sceneProperties, Action callback)
    {
        yield return StartCoroutine(ShowLoading(sceneProperties));

        //Debug.Log("called again");       
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, sceneProperties.loadSceneMode);

        float dummyProgress = 0.1f;

        string loadingText = loadingTextFactory.GetLoadingText(sceneName);

        while (op.progress < 1)
        {
            float progress = (op.progress < 9) ? dummyProgress : op.progress;            
            dummyProgress += 0.1f;
            dummyProgress = Mathf.Clamp(dummyProgress, 0f, 0.9f);

            loadingView?.SetLoading(progress, loadingText);
            yield return null;
        }

        loadingView?.SetLoading(1f, loadingText);
        yield return new WaitForSeconds(2f);

        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }

        callback?.Invoke();
    }

    public static void UnloadScene(SceneProperties sceneproperty, Action callback = null)
    {
        m_Instance.StartCoroutine(m_Instance.WaitForUnloadScene(sceneproperty, callback));
    }

    private IEnumerator ShowLoading(SceneProperties sceneProperties)
    {
        if (sceneProperties.showLoading)
        {
            loadingCanvas?.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            loadingCanvas?.SetActive(false);
        }
    }

    private IEnumerator WaitForUnloadScene(SceneProperties sceneProperties, Action callback = null) 
    {
        AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(sceneProperties.sceneName);        
        while (sceneUnload != null && !sceneUnload.isDone)
        {
            //loadingCanvas?.SetActive(true);
            yield return null;
        }

        callback?.Invoke();
        yield return new WaitForSeconds(1f);
        //bundle.Unload(true);
        //loadingCanvas.SetActive(false);
    }
}

[System.Serializable]
public class SceneProperties
{
    public string sceneName;
    public LoadSceneMode loadSceneMode;
    public bool isAsync;
    public bool showLoading;

    public SceneProperties()
    {
    }

    public SceneProperties(string sceneName)
    {
        this.sceneName = sceneName;
        loadSceneMode = LoadSceneMode.Single;
        isAsync = true;
        showLoading = true;
    }
}
