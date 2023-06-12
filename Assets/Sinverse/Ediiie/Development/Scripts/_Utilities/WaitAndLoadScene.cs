using UnityEngine;

public class WaitAndLoadScene : MonoBehaviour
{
    [SerializeField] private SceneProperties sceneProperties;

    private void OnEnable()
    {
        SceneController.LoadScene(sceneProperties);
    }
}
