using Notification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private void Awake()
    {
        NotificationUI.OnAddToCanvas += AddToCanvas;
    }

    private void OnDestroy()
    {
        NotificationUI.OnAddToCanvas -= AddToCanvas;
    }

    private void AddToCanvas(Transform transform)
    {
        transform.SetParent(this.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
