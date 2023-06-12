using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;

namespace VavilichevGD.UI
{
    public enum Layer
    {
        Default,
        AlwaysOnTop,
        Dialogue,
        Tutorial,
        FX
    }

    public abstract class UIElement : MonoBehaviour
    {

        #region DELEGATES
        
        public delegate void UIElementStateChangeHandler(UIElement uiElement, bool activated);
        public event UIElementStateChangeHandler OnStateChanged;

        public delegate void UIUIElementHandler(UIElement uiElement);
        public event UIUIElementHandler OnUIElementClosedCompletelyEvent;

        #endregion
        
        
        [SerializeField]
        protected Layer m_layer = Layer.Default;

        [SerializeField]
        protected bool m_preCached;

        public Layer layer => m_layer;
        public bool preCached => m_preCached;
        public bool isActive { get; protected set; }

//        protected Canvas canvas;

        protected const bool ACTIVATED = true;
        protected const bool DEACTIVATED = false;



        public virtual void Initialize()
        {
        }

        protected virtual void Awake()
        {
            Game.OnGameInitialized += OnGameInitialized;
            Game.OnGameReady += this.OnGameReady;
            Game.OnGameStart += this.OnGameStart;
//            if (preCached)
//            {
//                canvas = gameObject.GetComponent<Canvas>();
//                if (!canvas)
//                    canvas = gameObject.AddComponent<Canvas>();
//                GraphicRaycaster raycaster = gameObject.GetComponent<GraphicRaycaster>();
//                if (!raycaster)
//                    gameObject.AddComponent<GraphicRaycaster>();
//            }
        }

        private void OnGameInitialized(Game game)
        {
            Game.OnGameInitialized -= OnGameInitialized;
            this.OnGameInitialized();
        }

        protected virtual void OnGameInitialized()
        {
        }

        private void OnGameReady(Game obj)
        {
            Game.OnGameReady -= this.OnGameReady;
            this.OnGameReady();
        }

        protected virtual void OnGameReady()
        {
        }

        private void OnGameStart(Game obj)
        {
            Game.OnGameStart -= this.OnGameStart;
            this.OnGameStart();
        }

        protected virtual void OnGameStart()
        {
        }

        protected virtual void Start()
        {
            if (!preCached)
                NotifyAboutStateChanged(ACTIVATED);
            this.OnStart();
        }
        
        protected virtual void OnStart() { }

        public virtual void Show() {
            this.OnPreShow();
            this.isActive = true;
            
            if (preCached) {
                transform.SetAsLastSibling();
                gameObject.SetActive(true);
//                canvas.enabled = true;
            }

            this.OnPostShow();
            NotifyAboutStateChanged(ACTIVATED);
        }

        protected virtual void OnPreShow() { }
        protected virtual void OnPostShow() { }

        public virtual void Hide()
        {
            HideInstantly();
        }

        public void HideInstantly()
        {
            if (preCached)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
            this.OnHiddenCompletely();
            this.isActive = false;
            NotifyAboutStateChanged(DEACTIVATED);
            this.OnUIElementClosedCompletelyEvent?.Invoke(this);
        }
        
        protected virtual void OnHiddenCompletely() { }

        protected virtual void NotifyAboutStateChanged(bool activated)
        {
            OnStateChanged?.Invoke(this, activated);
            UIController.NotifyAboutStateChanged();
        }

        protected T GetInteractor<T>() where T : IInteractor
        {
            return Game.GetInteractor<T>();
        }

        protected IEnumerable<T> GetInteractors<T>() where T : IInteractor
        {
            return Game.GetInteractors<T>();
        }
    }
}