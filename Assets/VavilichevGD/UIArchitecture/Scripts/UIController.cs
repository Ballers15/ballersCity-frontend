using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using SinSity;
using SinSity.Core;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.UI {
    public class UIController : MonoBehaviour {

        #region DELEGATES

        public delegate void UIStateChangeHandler();
        public static event UIStateChangeHandler OnUIStateChanged;

        public delegate void UIControllerHandler(UIController uiController);
        public event UIControllerHandler OnAllCachedElementsCreatedEvent;

        #endregion
        
        [SerializeField] protected Transform layerContainerBase;
        [SerializeField] protected Transform layerContainerAlwaysOnTop;
        [SerializeField] protected Transform layerContainerDialogues;
        [SerializeField] protected Transform layerContainerTutorial;
        [SerializeField] protected Transform layerContainerFX;
        [SerializeField] private Transform uiControllersContainer;
        [Space] 
        [SerializeField] private Canvas m_mainCanvas;

        protected Dictionary<Type, string> uiElementPathsMap;
        protected Dictionary<Type, UIElement> uiCachedElementsMap;
        protected Dictionary<Type, UIElement> createdElementsMap;

        protected const string PREFABS_FOLDER = "UIElements";
        protected const string PREFAB_TUTORIAL_LAYER_NAME = "UILayerTutorial";

        protected bool isLayerTutorialInitialized => layerTutorial != null;
        protected UILayerTutorial layerTutorial;

        public static Camera cameraUI { get; protected set; }
        public static Camera cameraMain { get; protected set; }

        public static Vector3 GetPositionRelativeUICamera(IObjectEcoClicker objectEcoClicker) {
            return objectEcoClicker.GetPositionRelativeCameraUI(cameraUI, cameraMain);
        }


        public static void NotifyAboutStateChanged() {
            OnUIStateChanged?.Invoke();
        }

        public Coroutine Initialize() {
            return StartCoroutine(InitializeRoutine());
        }

        protected IEnumerator InitializeRoutine() {
            cameraUI = gameObject.GetComponentInChildren<Camera>();
            cameraMain = Camera.main;
            uiCachedElementsMap = new Dictionary<Type, UIElement>();
            uiElementPathsMap = new Dictionary<Type, string>();
            createdElementsMap = new Dictionary<Type, UIElement>();
            
            UIElement[] uiElements = Resources.LoadAll<UIElement>(PREFABS_FOLDER);
            
            foreach (UIElement uiElement in uiElements) {
                if (uiElement.preCached)
                    CreateAndCache(uiElement);
                    
                Type type = uiElement.GetType();
                uiElementPathsMap.Add(type, uiElement.name);
                yield return null;
            } 
            
            Logging.Log($"UI is initialized. Total elements count = {uiElementPathsMap.Count}, pre-cached = {uiCachedElementsMap.Count}");
            this.OnAllCachedElementsCreatedEvent?.Invoke(this);
        }

        protected void CreateAndCache<T>(T pref) where T : UIElement {
            Transform container = GetContainer(pref.layer);
            
            T createdElement = Instantiate(pref, container);
            createdElement.name = pref.name;
            Type type = pref.GetType();
            uiCachedElementsMap.Add(type, createdElement);
            createdElement.Initialize();
            createdElement.HideInstantly();
            createdElementsMap.Add(type, createdElement);
        }

        protected Transform GetContainer(Layer layer) {
            switch (layer) {
                case Layer.Default:
                    return layerContainerBase;
                case Layer.Dialogue:
                    return layerContainerDialogues;
                case Layer.AlwaysOnTop:
                    return layerContainerAlwaysOnTop;
                case Layer.Tutorial:
                    return layerContainerTutorial;
                case Layer.FX:
                    return layerContainerFX;
                default:
                    return layerContainerBase;
            }
        }

        public T Show<T>() where T : UIElement {
            Type type = typeof(T);
            if (uiCachedElementsMap.ContainsKey(type)) {
                UIElement uiElement = uiCachedElementsMap[type];
                uiElement.Show();
                uiElement.OnStateChanged += OnStateChanged;
                return (T)uiElement;
            }

            if (uiElementPathsMap.ContainsKey(type)) {
                string path = uiElementPathsMap[type];
                string fullPath = $"{PREFABS_FOLDER}/{path}";
                T pref = Resources.Load<T>(fullPath);
                Transform container = GetContainer(pref.layer);
                T createdElement = Instantiate(pref, container);
                createdElement.name = pref.name;
                createdElement.Initialize();
                createdElement.Show();
                Resources.UnloadUnusedAssets();
                createdElement.OnStateChanged += OnStateChanged;
                createdElementsMap[type] = createdElement;
                return createdElement;
            }
            
            Logging.LogError($"There is no element initialized in cached maps or paths. Type: {type}");
            return null;
        }

        private void OnStateChanged(UIElement uielement, bool activated) {
            NotifyAboutStateChanged();
            if (!activated) {
                uielement.OnStateChanged -= OnStateChanged;
                if (!uielement.preCached)
                    createdElementsMap.Remove(uielement.GetType());
            }
        }


        public void HighlightObject(RectTransform uiElementRectTransform, UITutorialStep tutorialStep) {
            if (!isLayerTutorialInitialized)
                ActivateTutorialLayer();
            
            layerTutorial.HighlightObject(uiElementRectTransform, tutorialStep);
        }

        public void ActivateTutorialLayer() {
            if (layerTutorial != null)
                return;

            UILayerTutorial pref = Resources.Load<UILayerTutorial>(PREFAB_TUTORIAL_LAYER_NAME);
            layerTutorial = Instantiate(pref, layerContainerTutorial);
            layerTutorial.name = pref.name;
            Resources.UnloadUnusedAssets();
        }

        public void DeactivateTutorialLayer() {
            if (isLayerTutorialInitialized)
                Destroy(layerTutorial.gameObject);
        }

        public RectTransform GetRectTransform(string path) {
            Transform child = layerContainerBase.Find(path);
            if (child != null)
                return child as RectTransform;

            child = layerContainerDialogues.Find(path);
            if (child != null)
                return child as RectTransform;

            child = layerContainerAlwaysOnTop.Find(path);
            if (child != null)
                return child as RectTransform;
            
            child = layerContainerTutorial.Find(path);
            if (child != null)
                return child as RectTransform;

            child = layerContainerFX.Find(path);
            
            if (!child)
                Logging.Log($"Cannot find object: {path}");

            return child as RectTransform;
        }

        public T GetUIElement<T>() where T : UIElement{
            Type type = typeof(T);
            if (createdElementsMap.ContainsKey(type))
                return (T)createdElementsMap[type];
            return null;
        }

        public bool OnlyGameplayScreenOpened() {
            const int maxOpenedScreens = 1;
            int openedScreens = 0;
            foreach (KeyValuePair<Type,UIElement> pair in createdElementsMap) {
                if (pair.Value.layer == Layer.FX)
                    continue;
                
                if (pair.Value.isActive) {
                    openedScreens++;
                }
            }

            if (openedScreens > maxOpenedScreens)
                return false;
            
            foreach (KeyValuePair<Type,UIElement> pair in uiCachedElementsMap) {
                if (pair.Value.layer == Layer.FX)
                    continue;
                
                if (pair.Value.isActive) {
                    openedScreens++;
                }
            }

            return openedScreens <= maxOpenedScreens && !BluePrint.bluePrintModeEnabled;
        }

        public bool AnyScreenEnabled() {
            foreach (KeyValuePair<Type,UIElement> pair in createdElementsMap) {
                if (pair.Value is UIScreenGamePlay)
                    continue;
                
                if (pair.Value.layer == Layer.FX)
                    continue;

                if (pair.Value.isActive)
                    return true;
            }
            
            
            foreach (KeyValuePair<Type,UIElement> pair in uiCachedElementsMap) {
                if (pair.Value is UIScreenGamePlay)
                    continue;
                
                if (pair.Value.layer == Layer.FX)
                    continue;

                if (pair.Value.isActive)
                    return true;
            }

            return BluePrint.bluePrintModeEnabled;
        }

        public bool IsActiveUIElement<T>() where T : UIElement {
            Type type = typeof(T);
            if (uiCachedElementsMap.ContainsKey(type))
                return uiCachedElementsMap[type].isActive;
            return false;
        }

        private void OnEnable() {
            InputManager.OnBackKeyClicked += OnBackKeyClicked;
        }

        private void OnBackKeyClicked() {
            if (OnlyGameplayScreenOpened())
                Show<UIPopupExit>();
        }

        private void OnDisable() {
            InputManager.OnBackKeyClicked -= OnBackKeyClicked;
        }

        public void SetVisibleUI(bool isVisible) {
            this.m_mainCanvas.gameObject.SetActive(isVisible);
        }
        
        public T GetUIController<T>() where T : MonoBehaviour {
            var uiController = uiControllersContainer.GetComponentInChildren<T>();
            
            if (uiController == null) {
                throw new Exception($"UIController of type {typeof(T)} doesn't found!");
            }

            return uiController;
        }
    }
}