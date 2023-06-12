using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class NetworkDetector : MonoBehaviour
{
    public static System.Action OnNetworkUnreachable;

    public static bool IsNetworkNotReachable {
        get {
            return Application.internetReachability == UnityEngine.NetworkReachability.NotReachable;
        }
    }

    private void Awake()
    {
        //StartCoroutine(NetworkReachability());
    }

    private IEnumerator NetworkReachability()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        if(IsNetworkNotReachable)
        {
           OnNetworkUnreachable?.Invoke();
        }
        
        StartCoroutine(NetworkReachability());
    }
}
