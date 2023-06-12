using UnityEngine;
using UnityEngine.UI;

public class TokenAmount : MonoBehaviour
{
    void OnEnable()
    {
        var tokens = PlayerPrefs.GetFloat("tokens").ToString("0.00");
        this.GetComponent<Text>().text = tokens;
    }
}
