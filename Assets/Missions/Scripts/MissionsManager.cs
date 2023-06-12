using System.Collections;
using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker {
	public class MissionsManager : MonoBehaviour {
		public static MissionsManager instance { get; private set; }
		public static bool isInitialized { get { return instance != null; } }

		[SerializeField]
		private Mission[] missionsPool;

		public Mission missionCurrent { get; private set; }

		private void Awake() {
			CreateSingleton();
		}

		private void CreateSingleton() {
			instance = this;
		}

		private IEnumerator Start() {
//			while (!IdleManager.allObjectsRegistered)
				yield return null;
			LateInitialize();
		}

		private void LateInitialize() {
			Mission actualMission = GetActualMission();
			missionCurrent = actualMission;
			if (actualMission)
				actualMission.Start();
		}

		public Mission GetActualMission() {
			Mission mission = GetMissionMustBeRewarded();
			if (!mission)
				mission = GetMissionStarted();

			if (mission)
				return mission;
			else if (missionsPool[missionsPool.Length - 1].state == MissionState.Complete)
				return null;
			else
				return missionsPool[0];
		}

		private Mission GetMissionMustBeRewarded() {
			foreach (Mission mission in missionsPool) {
				if (mission.state == MissionState.CompleteNotReceivedReward)
					return mission;
			}
			return null;
		}

		private Mission GetMissionStarted() {
			foreach (Mission mission in missionsPool) {
				if (mission.state == MissionState.Started)
					return mission;
			}
			return null;
		}

		public Mission StartNextMission() {
			int index = GetCurrentMissionIndex();
			index++;

			CleanMission();

			if (index < missionsPool.Length) {
				missionCurrent = missionsPool[index];
				missionCurrent.Start();
			}
			return missionCurrent;
		}

		private int GetCurrentMissionIndex() {
			for (int i = 0; i < missionsPool.Length; i++) {
				if (missionsPool[i] == missionCurrent)
					return i;
			}
			throw new System.Exception(string.Format("Can not find index of mission: {0}", missionCurrent));
		}

		private void CleanMission() {
			if (missionCurrent != null) {
				missionCurrent.CleanInfo();
				missionCurrent = null;
			}
		}
	}
}