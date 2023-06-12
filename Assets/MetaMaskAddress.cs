using UnityEngine;
using UnityEngine.UI;

public class MetaMaskAddress : MonoBehaviour
{
    void OnEnable()
    {
        var playerId = PlayerPrefs.GetString("Account");
        this.GetComponent<Text>().text = playerId;
    }
}
