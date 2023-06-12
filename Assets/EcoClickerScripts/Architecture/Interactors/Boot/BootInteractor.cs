using System.Collections;
using SinSity.Scripts;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Domain {
    public sealed class BootInteractor : Interactor {
        private readonly IBootLauncher firebaseLauncher = new FirebaseLauncher();

        private readonly IBootLauncher facebookLauncher = new FacebookLauncher();
        public override bool onCreateInstantly { get; } = true;

        protected override IEnumerator InitializeRoutineNew() {
            yield return base.InitializeRoutineNew();
            yield return this.firebaseLauncher.Launch();
            yield return this.facebookLauncher.Launch();
        }

        public override void OnReady() {
            base.OnReady();
            Coroutines.StartRoutine(Routine());
        }


        private IEnumerator Routine() {
            yield return new WaitForSecondsRealtime(2);
            CommonAnalytics.LogGameStarted();
        }
    }
}