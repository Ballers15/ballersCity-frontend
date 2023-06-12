using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerErrorScreen : MonoBehaviour
{

    #region STATIC

    private static ServerErrorScreen instance { get; set; }
    private static bool isInitialized => instance != null;

    #endregion
    

    private const string PATH_PREFAB = "[SERVER ERROR SCREEN]";

    private void OnEnable() {
        //textVersion.text = $"v. {Application.version}";
    }

    public static void Show(bool instantly) {
        CreateSingletonIfNeed();
    }

    private static void OnScreenHided() {
        Destroy(instance.gameObject);
    }

    private static void CreateSingletonIfNeed() {
        if (instance == null) {
            var prefab = Resources.Load<ServerErrorScreen>(PATH_PREFAB);
            instance = Instantiate(prefab);
            instance.transform.SetAsLastSibling();
        }
    }

    public static void Hide() {
        if (!isInitialized)
            return;
    }

    private void Awake() {
        Initialize();
    }

    private void Initialize() {
        //animator = gameObject.GetComponentInChildren<LoadingScreenAnimator>(true);
    }
}
