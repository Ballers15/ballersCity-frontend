using System.Collections;
using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class FXTimeBoosterGenerator : FXGenerator<FXTimeBooster> {
        private static FXTimeBoosterGenerator instance;

        [SerializeField] private Transform target;

        private const float FX_APPEARING_DURATION = 0.5f;
        private const float OFFSET = 0.3f;


        protected override bool CreateSingleton() {
            if (instance == null) {
                instance = this;
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        protected override void InitFXPool() {
            fxPool = new Pool<FXTimeBooster>(pref, poolCount, transform);
            FXTimeBooster[] fxs = fxPool.GetAllElements();
            foreach (FXTimeBooster fx in fxs)
                fx.SetTarget(target);
        }


        public static void MakeFX(IObjectEcoClicker objectEcoClicker, Product productTimeBooster, int count) {
            Vector3 validWorldPosition = GetValidPosition(objectEcoClicker);
            int fxCount = instance.ClampWithFreeElements(count);
            instance.StartCoroutine(instance.CreatePoolOfFXs(validWorldPosition, productTimeBooster, fxCount, count));
        }

        private IEnumerator CreatePoolOfFXs(Vector3 validWoldPosition, Product productTimeBooster, int fxCount,
            int timeBoostersReward) {
            int timeBoostersRewardCountAverage = timeBoostersReward / fxCount;
            bool dividedEqually = timeBoostersReward % fxCount == 0;

            int timeBoostersSum = timeBoostersReward;
            float timeBetweenFXs = FX_APPEARING_DURATION / fxCount;
            WaitForSeconds frame = new WaitForSeconds(timeBetweenFXs);

            for (int i = 0; i < fxCount; i++) {
                int timeBoostersRewardCountRewardRandom =
                    dividedEqually ? timeBoostersRewardCountAverage : timeBoostersRewardCountAverage + 1;
                if (timeBoostersRewardCountRewardRandom > timeBoostersSum)
                    timeBoostersRewardCountRewardRandom = timeBoostersSum;
                timeBoostersSum -= timeBoostersRewardCountRewardRandom;

                Vector3 position = GetRandomPositionAround(validWoldPosition, OFFSET);

                CreateFX(position, productTimeBooster, timeBoostersRewardCountRewardRandom);
                yield return frame;
            }
        }

        private static void CreateFX(Vector3 validWorldPosition, Product productCase, int casesCount) {
            ProductInfoTimeBooster infoTimeBooster = productCase.GetInfo<ProductInfoTimeBooster>();
            Sprite spriteIcon = infoTimeBooster.GetSpriteIcon();

            FXTimeBooster fx = instance.fxPool.GetFreeElement();
            fx.SetStartPosition(validWorldPosition);
            fx.SetIcon(spriteIcon);
            fx.SetReward(productCase, casesCount);
        }
    }
}