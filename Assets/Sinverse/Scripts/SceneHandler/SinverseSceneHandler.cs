using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sinverse
{
    public class SinverseSceneHandler : MonoBehaviour, IInit
    {
        private ISceneCallback Context;

        public void Init<T>(T Context)
        {
            this.Context = (ISceneCallback)(object)(Context);
            SceneManager.sceneLoaded += OnLoadedNewScene;    
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLoadedNewScene;
        }

        private void OnLoadedNewScene(Scene scene, LoadSceneMode sceneMode)
        {
            Context.LoadSceneCallback(scene, sceneMode);
        }

        public void LoadNewLevel(SceneProperties sceneProperties)
        {
            SceneController.LoadScene(sceneProperties);
        }
    }
}

public interface ISceneCallback
{
    public void LoadSceneCallback(Scene scene, LoadSceneMode sceneMode);
}
