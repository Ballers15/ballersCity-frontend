using UnityEngine;

public class BluePrintScheme : MonoBehaviour
{
    [SerializeField] private string m_idleObjectId;
    [SerializeField] private string m_zoneName;

    public string idleObjectId => m_idleObjectId;
    public string zoneName => m_zoneName;

    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
