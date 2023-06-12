using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Tools.Example {
    public class GameTimeExample : MonoBehaviour {

        [SerializeField] private Text textGameSessionInfoPrevous;
        [SerializeField] private Text textGameSessionInfoCurrent;
        [SerializeField] private Text textGameSessionTime;
        [SerializeField] private Text textUnscaledDeltaTime;
        [SerializeField] private Text textTimeBtwSessions;
        [Space] 
        [SerializeField] private Button btnPause;
        [SerializeField] private Text textBtnPause;
        
        
        
        private void Start() {
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine() {
            GameTimeRepository repository = new GameTimeRepository();
            yield return repository.InitializeRepository();
            
            GameTimeInteractor interactor = new GameTimeInteractor();
            yield return interactor.InitializeInteractor();
        }


        private void OnEnable() {
            GameTime.OnGameTimeInitialized += OnGameTimeInitialized;
            btnPause.onClick.AddListener(OnPauseBtnClicked);
        }

        private void OnGameTimeInitialized() {
            UpdateView();
        }

        private void UpdateView() {
            textGameSessionInfoPrevous.text =
                GameTime.isInitialized ?  $"{GameTime.gameTimeDataLastSession}" : "None";
            textGameSessionInfoCurrent.text =
                GameTime.isInitialized ? GameTime.gameTimeDataCurrentSession.ToString() : "None";

           

            textTimeBtwSessions.text = GameTime.isInitialized
                ? GameTime.timeSinceLastSessionEndedToCurrentSessionStartedSeconds.ToString()
                : "None";

            UpdateBtnPauseView();
        }

        private void UpdateBtnPauseView() {
            btnPause.interactable = GameTime.isInitialized;
            textBtnPause.text = GameTime.isPaused ? "Unpause" : "Pause";
        }
        

        private void OnPauseBtnClicked() {
            GameTime.SwitchPauseState();
            UpdateBtnPauseView();
        }

        private void Update() {
            textUnscaledDeltaTime.text = GameTime.unscaledDeltaTime.ToString(CultureInfo.InvariantCulture);
            textGameSessionTime.text = GameTime.timeSinceGameStarted.ToString(CultureInfo.InvariantCulture);
        }

        private void OnDisable() {
            GameTime.OnGameTimeInitialized -= OnGameTimeInitialized;
            btnPause.onClick.RemoveListener(OnPauseBtnClicked);
        }
    }
}