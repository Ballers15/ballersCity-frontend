using System.Collections;
using System.Runtime.CompilerServices;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class FXCasesGenerator : FXGenerator<FXCase> {
        private static FXCasesGenerator instance;

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
            fxPool = new Pool<FXCase>(pref, poolCount, transform);
            FXCase[] fxs = fxPool.GetAllElements();
            foreach (FXCase fx in fxs)
                fx.SetTarget(target);
        }


        public static void MakeFX(IObjectEcoClicker objectEcoClicker, Product productCase, int count) {
            Vector3 validWorldPosition = GetValidPosition(objectEcoClicker);
            int fxCount = instance.ClampWithFreeElements(count);
            instance.StartCoroutine(instance.CreatePoolOfFXs(validWorldPosition, productCase, fxCount, count));
        }
        
        private IEnumerator CreatePoolOfFXs(Vector3 validWoldPosition, Product productCase, int fxCount, int casesReward) {
            int casesRewardCountAverage = casesReward / fxCount;
            bool dividedEqually = casesReward % fxCount == 0;
            
            int casesSum = casesReward;
            float timeBetweenFXs = FX_APPEARING_DURATION / fxCount;
            WaitForSeconds frame = new WaitForSeconds(timeBetweenFXs);

            for (int i = 0; i < fxCount; i++) {
                int casesRewardCountRewardRandom = dividedEqually ? casesRewardCountAverage : casesRewardCountAverage + 1;
                if (casesRewardCountRewardRandom > casesSum)
                    casesRewardCountRewardRandom = casesSum;
                casesSum -= casesRewardCountRewardRandom;
                
                Vector3 position = GetRandomPositionAround(validWoldPosition, OFFSET);
                
                CreateFX(position, productCase, casesRewardCountRewardRandom);
                yield return frame;
            }
        }

        private static void CreateFX(Vector3 validWorldPosition, Product productCase, int casesCount) {
            ProductInfoCase infoCase = productCase.GetInfo<ProductInfoCase>();
            Sprite spriteIcon = infoCase.GetSpriteIcon();
            
            FXCase fx = instance.fxPool.GetFreeElement();
            fx.SetStartPosition(validWorldPosition);
            fx.SetIcon(spriteIcon);
            fx.SetReward(productCase, casesCount);
        }
    }
}