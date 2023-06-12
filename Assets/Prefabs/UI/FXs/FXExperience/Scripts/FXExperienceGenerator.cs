using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class FXExperienceGenerator : FXGenerator<FXExperience> {
        private static FXExperienceGenerator instance;
        
        [SerializeField] private Transform target;

        private const float FX_APPEARING_DURATION = 0.5f;
        private const float OFFSET = 0.3f;
        private const int FX_COUNT_MAX = 7;
        

        protected override bool CreateSingleton() {
            if (instance == null) {
                instance = this;
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        protected override void InitFXPool() {
            fxPool = new Pool<FXExperience>(pref, poolCount, transform);
            FXExperience[] fxs = fxPool.GetAllElements();
            foreach (FXExperience fx in fxs)
                fx.SetTarget(target);
        }

        
        protected static void MakeFX(Vector3 validWorldPosition, int fxCount, int expReward) {
            instance.StartCoroutine(instance.CreatePoolOfFXs(validWorldPosition, fxCount, expReward));
        }
        
        public static void MakeFX(IObjectEcoClicker objectEcoClicker, int expReward) {
            int fxCount = instance.GetFxCount(expReward);
            Vector3 validPostion = GetValidPosition(objectEcoClicker);
            MakeFX(validPostion, fxCount, expReward);
        }

        private int GetFxCount(int value) {
            int clampedMax = Mathf.Min(value, FX_COUNT_MAX);
            int clampedByPool = this.ClampWithFreeElements(clampedMax);
            return clampedByPool;
        }

        private IEnumerator CreatePoolOfFXs(Vector3 validWoldPosition, int fxCount, int expReward) {
            int expRewardCountAverage = Mathf.CeilToInt(expReward / fxCount);
            bool dividedEqually = expReward % fxCount == 0;
            
            int expSum = expReward;
            float timeBetweenFXs = FX_APPEARING_DURATION / fxCount;
            WaitForSeconds frame = new WaitForSeconds(timeBetweenFXs);

            for (int i = 0; i < fxCount; i++) {
                int expRewardCountRewardRandom = dividedEqually ? expRewardCountAverage : Random.Range(expRewardCountAverage, expRewardCountAverage + 2);
                if (expRewardCountRewardRandom > expSum)
                    expRewardCountRewardRandom = expSum;
                
                if (expRewardCountRewardRandom == 0)
                    yield break;
                
                expSum -= expRewardCountRewardRandom;
                
                Vector3 position = GetRandomPositionAround(validWoldPosition, OFFSET);
                
                CreateFX(position, expRewardCountRewardRandom);
                yield return frame;
            }
        }

        private void CreateFX(Vector3 worldPosition, int expReward) {
            FXExperience fx = instance.fxPool.GetFreeElement();
            fx.SetStartPosition(worldPosition);
            fx.SetReward((ulong)expReward);
        }
    }
}