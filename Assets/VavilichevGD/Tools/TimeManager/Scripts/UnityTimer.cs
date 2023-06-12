using System.Collections;
using UnityEngine;
using VavilichevGD.Timing;

public class UnityTimer {

    public delegate void TimerValueChangedHandler(UnityTimer unityTimer, float value);
    public event TimerValueChangedHandler OnValueChanged;

    public delegate void TimerFinishHandler(UnityTimer unityTimer);
    public event TimerFinishHandler OnTimerFinished;
    
    private const string TIMER_MONO_OBJECT_NAME = "UnityTimerMono";
    private Coroutine timerRoutine;
    private MonoBehaviour mono;
    private float timerSeconds;

    public float value => timerSeconds;

    public bool isActive {
        get;
        private set;
    }

    public UnityTimer(float seconds) {
        timerSeconds = seconds;
        InitializeMono();
    }

    public UnityTimer() {
        InitializeMono();
    }

    private void InitializeMono() {
        UnityTimerMono timerMono = Object.FindObjectOfType<UnityTimerMono>();
        
        if (!timerMono)
            timerMono = new GameObject(TIMER_MONO_OBJECT_NAME).AddComponent<UnityTimerMono>();
        mono = timerMono;
        Object.DontDestroyOnLoad(mono.gameObject);
    }

    public void SetTimer(float seconds)
    {
        timerSeconds = seconds;
    }

    public void Start() {
        if (timerRoutine != null) {
            Debug.LogError("Cannot start timer while it is working");
            return;
        }

        timerRoutine = mono.StartCoroutine(TimerRoutine());
    }

    public void Start(float seconds) {
        if (timerRoutine != null) {
            Debug.LogError("Cannot start timer while it is working");
            return;
        }

        timerSeconds = seconds;
        timerRoutine = mono.StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine() {
        isActive = true;
        WaitForSecondsRealtime oneSec = new WaitForSecondsRealtime(1f);
        NotifyAboutValueChanged(timerSeconds);

        while (timerSeconds > 0) {
            yield return oneSec;
            timerSeconds--;
            NotifyAboutValueChanged(timerSeconds);
        }

        isActive = false;
    }

    private void NotifyAboutValueChanged(float seconds) {
        OnValueChanged?.Invoke(this, seconds);
        if (seconds <= 0)
            OnTimerFinished?.Invoke(this);
    }


    public void Stop() {
        timerSeconds = 0f;
        if (timerRoutine != null && mono != null)
            mono.StopCoroutine(timerRoutine);
        timerRoutine = null;
        isActive = false;
    }

    public override string ToString() {
        int minutes = Mathf.FloorToInt(timerSeconds / 60);
        int seconds = Mathf.FloorToInt(timerSeconds % 60);
        string strMin = minutes < 10 ? $"0{minutes}" : minutes.ToString();
        string strSec = seconds < 10 ? $"0{seconds}" : seconds.ToString();
        return $"{strMin}:{strSec}";
    }
}
