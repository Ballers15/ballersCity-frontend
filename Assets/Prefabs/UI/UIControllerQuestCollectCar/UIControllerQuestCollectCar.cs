using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Domain.Utils;
using SinSity.Scripts;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using Random = UnityEngine.Random;

namespace SinSity.UI {
    public class UIControllerQuestCollectCar : UISceneElement {
        public event Action OnCarCollected;

        [SerializeField] private UICar car;
        [SerializeField] private PointWithIdleForPlacingCar[] pointsForCarWithIdles;
        [SerializeField] private int hidePeriodMin = 10;
        [SerializeField] private int hidePeriodMax = 20;
        [SerializeField] private int showPeriodMin = 10;
        [SerializeField] private int showPeriodMax = 20;

        private bool isActive;
        private int showedTimerValue;
        private int hiddenTimerValue;
        private int time;

        public void Activate() {
            isActive = true;
            hiddenTimerValue = Random.Range(hidePeriodMin, hidePeriodMax);
            showedTimerValue = Random.Range(showPeriodMin, showPeriodMax);
            time = 0;
            GameTime.OnSecondTickEvent += GameTimeOnSecondTick;
            car.OnCarClicked += OnCarClicked;
        }
        
        public void Deactivate() {
            isActive = false;
            GameTime.OnSecondTickEvent -= GameTimeOnSecondTick;
            car.OnCarClicked -= OnCarClicked;
        }
        
        private void GameTimeOnSecondTick() {
            if (!isActive) return;
            
            time += 1;
            TryHideOrShowCar();
        }

        private void TryHideOrShowCar() {
            if (car.IsShowing()) {
                if (!CanHideCar()) return;
                car.Hide();
            }
            else {
                if (!CanShowCar()) return;
                ShowCar();
            }
            time = 0;
        }
        
        private bool CanHideCar() {
            return time >= showedTimerValue && car.IsShowing();
        }

        private bool CanShowCar() {
            return time >= hiddenTimerValue && !car.IsShowing();
        }
        
        private void ShowCar() {
            var carPointPosition = GetRandomPointPosition();
            car.Show(carPointPosition);
        }

        private Vector2 GetRandomPointPosition() {
            var pointsWithBuiltIdle = GetPointsWithBuiltIdles();
            var randomPointIndex = Random.Range(0, pointsWithBuiltIdle.Length);
            return pointsWithBuiltIdle[randomPointIndex].point.position;
        }

        private PointWithIdleForPlacingCar[] GetPointsWithBuiltIdles() {
            var idlesInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            return pointsForCarWithIdles.Where(pointWithIdle => idlesInteractor.IdleObjectIsBuilt(pointWithIdle.idleObjectInfo.id)).ToArray();
        }
        
        private void OnCarClicked() {
            car.Hide();
            time = 0;
            OnCarCollected?.Invoke();
        }
        
        protected override void OnGameInitialized() {
            base.OnGameInitialized();
            car.Hide();
        }
        
        [Serializable]
        private struct PointWithIdleForPlacingCar {
            public IdleObjectInfo idleObjectInfo;
            public Transform point;
        }
    }
}

