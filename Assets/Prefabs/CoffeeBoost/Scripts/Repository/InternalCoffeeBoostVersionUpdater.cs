using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo {
    [CreateAssetMenu(
        fileName = "InternalCoffeeBoostVersionUpdater",
        menuName = "Repo/CoffeeBoost/New InternalCoffeeBoostVersionUpdater"
    )]
    public class InternalCoffeeBoostVersionUpdater : ScriptableObject, IVersionUpdater<CoffeeBoostStatistics> {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref CoffeeBoostStatistics data)
        {
            if (data != null)
            {
                return false;
            }

            data = new CoffeeBoostStatistics
            {
                version = Instantiate(this.scriptableVersion).value,
                isUnlocked = false,
            };

            return true;
        }
    }
}