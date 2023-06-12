using UnityEngine;

public class KeyDownEventListener : MonoBehaviour
{
    public KeyCode keyCode;
    IKeyEventListener keyEventListener => GetComponent<IKeyEventListener>();

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            keyEventListener.OnKeyPressed(keyCode);
        }
    }

}

public interface IKeyEventListener
{
    public void OnKeyPressed(KeyCode keyCode);
}