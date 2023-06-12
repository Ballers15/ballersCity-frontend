using System.Collections;
using UnityEngine;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIInteractorEcoClicker : UIInteractor {

        private const string UICONTROLLER_PREFAB_NAME = "[INTERFACE]";

        public override bool onCreateInstantly { get; } = false;

        protected override IEnumerator InitializeRoutineNew() {
            yield return base.InitializeRoutineNew();
            uiController.Show<UIScreenGamePlay>();
        }

        protected override UIController CreateUIController() {
            UIController pref = Resources.Load<UIController>(UICONTROLLER_PREFAB_NAME);
            UIController createdController = Object.Instantiate(pref);
            Object.DontDestroyOnLoad(createdController.gameObject);
            return createdController;
        }
    }
}