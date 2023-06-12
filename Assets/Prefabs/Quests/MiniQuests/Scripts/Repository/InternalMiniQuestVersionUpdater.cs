using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalMiniQuestVersionUpdater",
        menuName = "Repo/MinQuest/New InternalMiniQuestVersionUpdater"
    )]
    public sealed class InternalMiniQuestVersionUpdater : ScriptableObject, IVersionUpdater<MiniQuestData>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref MiniQuestData data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.BuildData();
            return true;
        }

        private MiniQuestData BuildData()
        {
            return new MiniQuestData
            {
                isUnlocked = false,
                version = Instantiate(this.scriptableVersion).value
            };
        }
    }
}