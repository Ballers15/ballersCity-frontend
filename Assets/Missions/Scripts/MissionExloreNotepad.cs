using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionExloreNotepad", menuName = "Mission/ExloreNotepad")]
	public class MissionExloreNotepad : Mission {

		private string prefKeyTab1 { get { return string.Format("{0}_TAB_OPENED_1", titleCode); } }
		private string prefKeyTab2{ get { return string.Format("{0}_TAB_OPENED_2", titleCode); } }
		private string prefKeyTab3 { get { return string.Format("{0}_TAB_OPENED_3", titleCode); } }
		private string prefKeyTab4 { get { return string.Format("{0}_TAB_OPENED_4", titleCode); } }

		private bool tabOpened1 {
			get {
				return Loader.LoadBool(prefKeyTab1, false);
			}
			set {
				Loader.SetBool(prefKeyTab1, value);
			}
		}
		private bool tabOpened2 {
			get {
				return Loader.LoadBool(prefKeyTab2, false);
			}
			set {
				Loader.SetBool(prefKeyTab2, value);
			}
		}
		private bool tabOpened3 {
			get {
				return Loader.LoadBool(prefKeyTab3, false);
			}
			set {
				Loader.SetBool(prefKeyTab3, value);
			}
		}
		private bool tabOpened4 {
			get {
				return Loader.LoadBool(prefKeyTab4, false);
			}
			set {
				Loader.SetBool(prefKeyTab4, value);
			}
		}

		private int openedCount {
			get {
				int count = 0;
				if (tabOpened1)
					count++;
				if (tabOpened2)
					count++;
				if (tabOpened3)
					count++;
				if (tabOpened4)
					count++;
				return count;
			}
		}

		protected override float GetProgress() {
			float step = 0.25f;
			return Mathf.Clamp01(openedCount * step);
		}

		public override string ToStringProgress() {
			int max = 4;
			return string.Format("{0}/{1}", openedCount, max);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
				//UINotepadTabSwitcher.OnTabOpened += UINotepadTabSwitcher_OnTabOpened;
			}
		}

		private void UINotepadTabSwitcher_OnTabOpened(int number) {
			if (number == 1)
				tabOpened1 = true;
			else if (number == 2)
				tabOpened2 = true;
			else if (number == 3)
				tabOpened3 = true;
			else if (number == 4)
				tabOpened4 = true;

			if (tabOpened1 && tabOpened2 && tabOpened3 && tabOpened4)
				Finish();
		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
				//UINotepadTabSwitcher.OnTabOpened -= UINotepadTabSwitcher_OnTabOpened;
			}
		}

		public override void CleanInfo() {
			Loader.Clean(prefKeyTab1);
			Loader.Clean(prefKeyTab2);
			Loader.Clean(prefKeyTab3);
			Loader.Clean(prefKeyTab4);
		}
	}
}