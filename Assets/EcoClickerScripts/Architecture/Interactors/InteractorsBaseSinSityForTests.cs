using System;
using System.Collections;
using System.Collections.Generic;
using IdleClicker.Gameplay;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta;
using SinSity.Meta.Quests;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Scripts {
    public class InteractorsBaseSinSityForTests : InteractorsBase
    {
        private List<Type> _interactorTypes;
        public InteractorsBaseSinSityForTests(List<Type> interactorTypes) {
            _interactorTypes = interactorTypes;
        }

        public override void CreateAllInteractors() {
            CreateInteractors(_interactorTypes);
        }

        public Coroutine FastInitialize() {
            return Coroutines.StartRoutine(FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine() {
            GameTimeInteractor gameTimeInteractor = GetInteractor<GameTimeInteractor>();
            yield return gameTimeInteractor.InitializeInteractor();
        }
    }
}