using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class FXGemsGenerator : FXGenerator<FXGems> {
        private static FXGemsGenerator instance;
        
        [SerializeField] private Transform target;

        private const float GEMS_APPEARING_DURATION = 0.5f;
        private const float OFFSET = 0.3f;
        private const int GEMS_COUNT_MAX = 7;
        

        protected override bool CreateSingleton() {
            if (instance == null) {
                instance = this;
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        protected override void InitFXPool() {
            fxPool = new Pool<FXGems>(pref, poolCount, transform);
            FXGems[] fxs = fxPool.GetAllElements();
            foreach (FXGems fx in fxs)
                fx.SetTarget(target);
        }

        
        public static void MakeFXLog(IObjectEcoClicker objectEcoClicker, int gemsReward) {
            int fxCount = Mathf.FloorToInt(Mathf.Log(gemsReward, 3f));
            Vector3 validPostion = GetValidPosition(objectEcoClicker);
            MakeFX(validPostion, fxCount, gemsReward, false);
        }
        
        private int GetFxCount(int value) {
            int clampedMax = Mathf.Min(value, GEMS_COUNT_MAX);
            int clampedByPool = this.ClampWithFreeElements(clampedMax);
            return clampedByPool;
        }
        
        protected static void MakeFX(Vector3 validWorldPosition, int fxCount, int gemsReward, bool zeroOffset) {
            instance.StartCoroutine(instance.CreatePoolOfFXs(validWorldPosition, fxCount, gemsReward, zeroOffset));
        }
        
        public static void MakeFX(IObjectEcoClicker objectEcoClicker, int gemsReward, bool zeroOffset = false) {
            int fxCount = instance.GetFxCount(gemsReward);
            Vector3 validPostion = GetValidPosition(objectEcoClicker);
            MakeFX(validPostion, fxCount, gemsReward, zeroOffset);
        }

        private IEnumerator CreatePoolOfFXs(Vector3 validWoldPosition, int fxCount, int gemsReward, bool zeroOffset) {
            int gemsRewardCountAverage = Mathf.CeilToInt(gemsReward / fxCount);
            bool dividedEqually = gemsReward % fxCount == 0;
            
            int gemsSum = gemsReward;
            float timeBetweenFXs = GEMS_APPEARING_DURATION / fxCount;
            WaitForSeconds frame = new WaitForSeconds(timeBetweenFXs);

            for (int i = 0; i < fxCount; i++) {
                int gemsRewardCountRewardRandom = dividedEqually ? gemsRewardCountAverage : Random.Range(gemsRewardCountAverage, gemsRewardCountAverage + 3);
                if (gemsRewardCountRewardRandom > gemsSum)
                    gemsRewardCountRewardRandom = gemsSum;
                
                if (gemsRewardCountRewardRandom == 0)
                    yield break;
                
                gemsSum -= gemsRewardCountRewardRandom;
                
                Vector3 position = zeroOffset ? validWoldPosition : GetRandomPositionAround(validWoldPosition, OFFSET);
                
                CreateFX(position, gemsRewardCountRewardRandom);
                yield return frame;
            }
        }

        private void CreateFX(Vector3 worldPosition, int gemsReward) {
            FXGems fx = instance.fxPool.GetFreeElement();
            fx.SetStartPosition(worldPosition);
            fx.SetReward(gemsReward);
        }
    }
}