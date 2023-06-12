using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionPlayGameFewDays", menuName = "Mission/PlayGameFewDays")]
	public class MissionPlayGameFewDays : Mission {
		[System.Serializable]
		public class MissionData {
			public DateTimeSerialized dateTimeSerialized;
			public int dayNumber;

			public bool isAlreadyStarted { get { return dateTimeSerialized.GetDateTime() != new System.DateTime(); } }

			public static MissionData empty {
				get {
					MissionData data = new MissionData();
					data.dateTimeSerialized = new DateTimeSerialized(new System.DateTime());
					data.dayNumber = 0;
					return data;
				}
			}
		}

		[Space]
		[SerializeField]
		private int daysCount;

		private string prefKeyProgress { get { return string.Format("MISSION_PLAY_COUNT_{0}", titleCode); } }
		private MissionData data {
			get {
				string strMissionData = Loader.LoadString(prefKeyProgress, JsonUtility.ToJson(MissionData.empty));
				return JsonUtility.FromJson<MissionData>(strMissionData);
			}
			set {
				string strMissionData = JsonUtility.ToJson(value);
				Loader.SetString(prefKeyProgress, strMissionData);
			}
		}


		protected override float GetProgress() {
			if (isFullyCompleted)
				return 1f;
			return Mathf.Clamp01((float)data.dayNumber / daysCount);
		}

		public override string ToStringProgress() {
			int dayNumber = data.dayNumber;
			if (isFullyCompleted)
				dayNumber = daysCount;
			return string.Format("{0}/{1}", dayNumber, daysCount);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
				MissionsManager.instance.StartCoroutine(StartRoutine());
			}
		}

		private IEnumerator StartRoutine() {
			while (!GameTime.isInitialized)
				yield return null;

			int differenceDays = GameTime.now.Day - data.dateTimeSerialized.GetDateTime().Day;
			if (differenceDays == 1) {
				MissionData newData = data;
				newData.dayNumber++;
				data = newData;
			}
			else {
				MissionData newData = data;
				newData.dayNumber = 1;
				data = newData;
			}
			//Debug.Log("Mission started. Days left: " + (daysCount - data.dayNumber).ToString());
			if (data.dayNumber >= daysCount)
				Finish();
		}

		public override void Finish() {
			if (!isFullyCompleted)
				base.Finish();
		}

		public override void CleanInfo() {
			Loader.Clean(prefKeyProgress);
		}
	}
}