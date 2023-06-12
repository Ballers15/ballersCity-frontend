using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalMainQuestVersionUpdater",
        menuName = "Repo/MainQuest/New InternalMainQuestVersionUpdater"
    )]
    public sealed class InternalMainQuestVersionUpdater : ScriptableObject, IVersionUpdater<MainQuestData>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref MainQuestData data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.BuildData();
            return true;
        }

        private MainQuestData BuildData()
        {
            return new MainQuestData
            {
                isUnlocked = false,
                version = Instantiate(this.scriptableVersion).value
            };
        }
    }
}