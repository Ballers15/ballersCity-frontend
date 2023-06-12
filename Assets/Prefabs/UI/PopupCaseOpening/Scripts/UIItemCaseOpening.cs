using UnityEngine;
using VavilichevGD;
using VavilichevGD.Tools;

namespace SinSity.UI {
	public class UIItemCaseOpening : AnimObject {

		public delegate void CaseOpenedHandler(UIItemCaseOpening uiCase);
		public event CaseOpenedHandler OnCaseOpened;
		public event CaseOpenedHandler OnCaseOpenedAnimaionOver;
		

		public void Handle_CaseOpened()
		{
			OnCaseOpened?.Invoke(this);
		}

		public void Handle_CaseOpenedAnimationOver() {
			OnCaseOpenedAnimaionOver?.Invoke(this);
		}

		public void Show() {
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}