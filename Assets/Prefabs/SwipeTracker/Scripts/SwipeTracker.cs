using System;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace IdleClicker
{
    public class SwipeTracker : MonoBehaviour
    {
        
        #region EVENTS

        public static event Action<SwipeData> OnSwipeEvent = delegate { };

        #endregion
    
        #region VARS

        private UIController uiController;
        private Vector2 fingerStartPosition;
        private SwipeTrackerProperties properties;
        private bool mouseButtonWasPressed;
        private DateTime dragStartTime;
        private Camera cam;
        private bool isActive;
        private bool isDragging;
        private float horizontalToleranse;

        #endregion

        
        void Start() {
            this.isActive = false;
            this.properties = Resources.Load<SwipeTrackerProperties>(SwipeTrackerProperties.PATH);

            Game.OnGameInitialized += OnInitialized;
        }

        private void OnInitialized(Game game) {
            Game.OnGameInitialized -= OnInitialized;

            var tutorialInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
            if (!tutorialInteractor.isTutorialCompleted)
                tutorialInteractor.OnTutorialCompleteEvent += this.OnTutorialComplete;
            else
                this.isActive = true;

            var uiInteractor = Game.GetInteractor<UIInteractor>();
            this.uiController = uiInteractor.uiController;
            this.cam = UIController.cameraUI;
        }

        private void OnTutorialComplete() {
            var tutorialInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
            tutorialInteractor.OnTutorialCompleteEvent -= this.OnTutorialComplete;
            this.isActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(this.isActive)
                this.SwipeDetectLogic();
        }

        private void SwipeDetectLogic()
        {
#if UNITY_EDITOR

            this.Drag(Input.mousePosition);
            
            if (Input.GetMouseButtonDown(0))
                this.StartDragging(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                this.StopDragging(Input.mousePosition);
            
 #else
            if (Input.touches.Length > 0) {
                var firsTouch = Input.touches[0];

                this.Drag(firsTouch.position);
                
                if (firsTouch.phase == TouchPhase.Began)
                    this.StartDragging(firsTouch.position);
                else if (firsTouch.phase == TouchPhase.Ended)
                    this.StopDragging(firsTouch.position);
            }
#endif
        }

        private void Drag(Vector3 pointerPosition) {
            if (this.isDragging) {
                var currentPosition = this.cam.ScreenToViewportPoint(pointerPosition);
                var currentHorizontalToleranse = Mathf.Abs(currentPosition.x - this.fingerStartPosition.x);
                this.horizontalToleranse = Mathf.Max(this.horizontalToleranse, currentHorizontalToleranse);
            }
        }

        private void StartDragging(Vector3 pointerPosition) {
            if (this.isDragging)
                return;
            
            this.isDragging = true;
            this.horizontalToleranse = 0f;
            this.fingerStartPosition = this.cam.ScreenToViewportPoint(pointerPosition);
            this.dragStartTime = DateTime.Now;
        }

        private void StopDragging(Vector3 pointerPosition) {
            if (!isDragging)
                return;
            
            this.isDragging = false;
            var fingerReleasePosition = this.cam.ScreenToViewportPoint(pointerPosition);
            this.TryToDetectSwipe(fingerReleasePosition);
        }

        private void TryToDetectSwipe(Vector3 fingerReleasePosition) {

            var dragDuration = (DateTime.Now - dragStartTime).TotalMilliseconds;
            // Logging.Log($"Drag duration: {dragDuration}");
            if (dragDuration > this.properties.maxTimeForSwipeMS)
                return;
            
            // Logging.Log($"Drag toleranse: {horizontalToleranse}");
            if (this.horizontalToleranse > this.properties.maxHorizontalTolerance)
                return;

            var verticalOffset = fingerReleasePosition.y - this.fingerStartPosition.y;
            // Logging.Log($"Drag verticalOffset: {verticalOffset}");
            if (Mathf.Abs(verticalOffset) < this.properties.minDistanceForSwipe)
                return;


            // Logging.Log($"Drag verticalOffset: {verticalOffset}");
            var direction = verticalOffset > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            SendSwipe(direction);
        }


        private void SendSwipe(SwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData()
            {
                Direction = direction,
                StartPosition = fingerStartPosition,
            };
            Logging.Log($"{swipeData.Direction.ToString()} {fingerStartPosition} /*fingerUpPosition*/");
            this.TryToSwitchScreens(swipeData);
            OnSwipeEvent?.Invoke(swipeData);
        }

        private void TryToSwitchScreens(SwipeData swipe) {
            if (uiController.OnlyGameplayScreenOpened() || BluePrint.bluePrintModeEnabled)
            {
                if(swipe.Direction == SwipeDirection.Up)
                    CameraController.instance.MoveDown(); 
                if(swipe.Direction == SwipeDirection.Down)
                    CameraController.instance.MoveUp();
            }
        }
    }

    public struct SwipeData
    {
        public Vector2 StartPosition;
        // public Vector2 EndPosition;
        public SwipeDirection Direction;
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}