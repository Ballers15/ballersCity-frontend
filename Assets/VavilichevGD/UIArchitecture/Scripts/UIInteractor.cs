using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.UI {
    public abstract class UIInteractor : Interactor {

        public UIController uiController { get; protected set; }
        protected Type uiElementPreviousType;
        protected UIElement uiElementCurrent;

        protected override IEnumerator InitializeRoutineNew() {
            uiController = CreateUIController();
            yield return uiController.Initialize();
            yield return null;
        }

        protected abstract UIController CreateUIController();

        public T ShowElement<T>() where T : UIElement {
            if (uiElementCurrent)
                uiElementPreviousType = uiElementCurrent.GetType();
            uiElementCurrent = uiController.Show<T>();
            return (T)uiElementCurrent;
        }

        public void ShowPrevious() {
            if (uiElementPreviousType == null)
                return;;
            
            //TODO: THIS IS EVIL!!! Почему не написать еще один метод ShowElement(Type type)
            MethodInfo method = this.GetType().GetMethod("ShowElement");
            MethodInfo genericMethod = method.MakeGenericMethod(uiElementPreviousType);
            genericMethod.Invoke(this, null);
        }

        public T GetUIElement<T>() where T : UIElement {
            return uiController.GetUIElement<T>();
        }

        public T GetUIController<T>() where T : MonoBehaviour {
            return uiController.GetUIController<T>();
        }
    }
}