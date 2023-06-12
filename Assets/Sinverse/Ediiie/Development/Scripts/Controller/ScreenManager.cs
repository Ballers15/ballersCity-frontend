using Ediiie.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Screens
{
    public class ScreenManager : MonoBehaviour
    {        
        [SerializeField] protected List<GameUI> referenceInScreen;        
        [SerializeField] protected GameObject depthCameraCanvasBlocker;
        [SerializeField] protected GameScreen defaultScreen;

        public static ScreenManager Instance;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject); //destroy new gameobject
            }
            else
            {
                Instance = this;
            }
        }

        protected virtual void Start()
        {
            GameScreen.currentScreen = defaultScreen;
        }           

        public static void ShowBackScreen()
        {
            ShowScreen(((GameScreenBackForth)GameScreen.currentScreen).backScreen);
        }

        public static void HideScreen(Screen screen)
        {
            GameUI gameUI = Instance.referenceInScreen.Find(x => x.screen == screen);
            gameUI.Show(false);
        }

        public static void ShowBlocker(bool on)
        {
            if (Instance.depthCameraCanvasBlocker != null)
            {
                Instance.depthCameraCanvasBlocker.SetActive(on);
            }
        }

        public static void ShowScreen(Screen screen, bool playSound = false)
        {          
            //Debug.Log("show screen : " + screen);
            if (playSound)
            {
                AudioManager.PlayAudio(screen.ToString());
            }

            if(screen == Screen.EXIT)
            {
                Application.Quit();
                return;
            }

            GameUI gameScreen = Instance.GetScreen(screen);
            bool isPopup = gameScreen.isOverlay;

            //hide last screen
            if (GameScreen.currentScreen != null)
            {
                if (!isPopup)
                {                    
                    GameScreen.currentScreen.gameObject.SetActive(false);
                    GameScreen.currentScreen = (GameScreen) gameScreen;
                }
            }

            ShowBlocker(gameScreen.hasBlockerUI);
            gameScreen.Show(true);
        }

        public static bool CheckScreenStatus(Screen screen)
        {
            GameUI gameScreen = Instance.GetScreen(screen);
            return (gameScreen.gameObject.activeSelf);

        }

        private GameUI GetScreen(Screen screenName)
        {
            Debug.Log(screenName);
            GameUI screenUI = referenceInScreen.Find(x => x.screen == screenName);
            return screenUI;
        }
    }
}