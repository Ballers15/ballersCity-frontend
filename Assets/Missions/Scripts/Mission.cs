using UnityEngine;

namespace IdleClicker {
	public enum MissionState {
		NotStarted,
		Started,
		CompleteNotReceivedReward,
		Complete
	}

	public abstract class Mission : ScriptableObject {

		[SerializeField]
		protected string m_titleCode;
		[SerializeField]
		protected string m_descCode;
		[SerializeField]
//		protected Reward reward;
//		[SerializeField, Tooltip("This number has different meanings for different classes;")]
//		protected RewardAttributes rewardAttributes;

		public string titleCode { get { return m_titleCode; } }
		public string descCode { get { return m_descCode; } }
		public MissionState state {
			get {
				return Loader.LoadEnum<MissionState>(prefKey, MissionState.NotStarted);
			}
			protected set {
				Loader.SetEnum(prefKey, value);
			}
		}
		public float progress { get { return GetProgress(); } }
		public bool readyForReward { get { return state == MissionState.CompleteNotReceivedReward; } }

		protected bool isFullyCompleted { get { return state == MissionState.Complete || state == MissionState.CompleteNotReceivedReward; } }
		protected string prefKey { get { return string.Format("MISSION_{0}", name); } }

		public delegate void MissionStartHandler(Mission mission);
		public static event MissionStartHandler OnMissionStarted;

		public delegate void MissionOverHandler(Mission mission);
		public static event MissionOverHandler OnMissionOver;

//		public delegate void MissionRewardTakenHandler(Reward reward);
//		public static event MissionRewardTakenHandler OnMissionRewardTaken;

		public delegate void MissionStateChangeHandler(MissionState state);
		public event MissionStateChangeHandler OnMissionStateChanged;

		protected abstract float GetProgress();

		public abstract string ToStringProgress();

		protected virtual bool IsMissionAlreadyComplete() {
			return false;
		}

		public virtual void Start() {
			if (!isFullyCompleted) {
				if (state == MissionState.NotStarted) {
					state = MissionState.Started;
					NotifyAboutMissionStarted();
					//Debug.Log("Mission started: " + descCode);
				}
			}
		}

		protected void NotifyAboutMissionStarted() {
			if (OnMissionStarted != null)
				OnMissionStarted(this);
		}

		public virtual void Finish() {
			if (!isFullyCompleted) {
				if (state == MissionState.Started) {
					state = MissionState.CompleteNotReceivedReward;
					NotifyAboutMissionOver();
					//Debug.Log("Mission finished: " + descCode);
				}
			}
		}

		protected void NotifyAboutMissionOver() {
			if (OnMissionOver != null)
				OnMissionOver(this);
		}

		public virtual void ApplyReward() {
			if (state == MissionState.CompleteNotReceivedReward) {
				state = MissionState.Complete;
//				reward.Apply(rewardAttributes);
				NotifyAbourMissionRewardWasTaken();
			}
		}

		protected void NotifyAbourMissionRewardWasTaken() {
//			if (OnMissionRewardTaken != null)
//				OnMissionRewardTaken(reward);
		}

		public virtual string GetTitle() {
			return titleCode;
		}

		public virtual string GetDescriptionCode() {
			return descCode;
		}

		public virtual string ToStringAttribute() {
			return "";
//			return reward.ToStringAttribute(rewardAttributes);
		}

		protected void NotifyAboutMissionStateChanged() {
			if (OnMissionStateChanged != null)
				OnMissionStateChanged(state);
		}

		public abstract void CleanInfo();

		public Sprite GetIconSprite() {
			return null;
//			return reward.iconSprite;
		}
	}
}