using System;
using UnityEngine;
using VavilichevGD;

public class ZoneStripes : AnimObject {
    
    private static readonly int hashClick = Animator.StringToHash("clicked");

    private void OnMouseDown() {
        SetBoolTrue(hashClick);
    }

    private void OnMouseUp() {
        SetBoolFalse(hashClick);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
            SetBoolTrue(hashClick);
        if (Input.GetMouseButtonUp(0))
            SetBoolFalse(hashClick);
    }
}
