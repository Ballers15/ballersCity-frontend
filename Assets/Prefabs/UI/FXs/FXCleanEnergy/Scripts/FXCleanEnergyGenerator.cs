using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class FXCleanEnergyGenerator : FXGenerator<FXCleanEnergy> {
        private static FXCleanEnergyGenerator instance;

        [SerializeField] private FXCleanEnergy prefFast;
        [SerializeField] private FXCleanEnergy prefSlow;
        [SerializeField] private Transform target;

        private static Pool<FXCleanEnergy> energyPoolFast;
        private static Pool<FXCleanEnergy> energyPoolSlow;

        private const int LOG_BASE = 10;
        private const float FX_APPEARING_DURATION = 0.5f;
        private const float OFFSET = 0.4f;
        private const int FX_COUNT_MAX = 7;

        protected override void Awake() { 
            if (CreateSingleton())
                PanelCollectedCurrencyLeaf.OnLeafCollected += OnLeafCollected;    
        }

        protected override bool CreateSingleton() {
            if (instance == null) {
                instance = this;
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        protected override void InitFXPool() {
            energyPoolFast = CreatePool(prefFast);
            energyPoolSlow = CreatePool(prefSlow);
        }

        private Pool<FXCleanEnergy> CreatePool(FXCleanEnergy pref) {
            Pool<FXCleanEnergy> pool = new Pool<FXCleanEnergy>(pref, poolCount, transform);
            FXCleanEnergy[] fxs = pool.GetAllElements();
            foreach (FXCleanEnergy fx in fxs)
                fx.SetTarget(target);
            return pool;
        }


        
        private void OnLeafCollected(Vector3 position, BigNumber rewardValue) {
            WorldObjectEcoClicker objectEcoClicker = new WorldObjectEcoClicker(position);
            MakeFXOneFast(objectEcoClicker, rewardValue);
        }
        
        public static void MakeFXFast(IObjectEcoClicker objectEcoClicker, BigNumber energyReward) {
            int fxCount = GetFXCount(energyReward, energyPoolFast);
            Vector3 validWorldPosittion = GetValidPosition(objectEcoClicker);
            MakeFX(energyPoolFast, validWorldPosittion, fxCount, energyReward);
        }

        private static int GetFXCount(BigNumber reward, Pool<FXCleanEnergy> pool) {
            double log = BigNumber.GetLog(reward, LOG_BASE);
            int clampedMax = Mathf.Min((int) log, FX_COUNT_MAX);
            int clampedByPool = instance.ClampWithFreeElements(clampedMax, pool);
            return clampedByPool;
        }

        private int ClampWithFreeElements(int value, Pool<FXCleanEnergy> pool) {
            int clampedValue = Mathf.Clamp(pool.GetFreeElementsCount(), 0, value);
            if (clampedValue == 0)
                throw new Exception($"There is no free elements in the pool: {this.name}");
            return clampedValue;
        }
        
        protected static void MakeFX(Pool<FXCleanEnergy> pool, Vector3 validWorldPosition, int fxCount, BigNumber energyReward) {
            instance.StartCoroutine(instance.CreatePoolOfFXs(pool, validWorldPosition, fxCount, energyReward));
        }

        public static void MakeFXSlow(IObjectEcoClicker objectEcoClicker, BigNumber energyReward) {
            int fxCount = GetFXCount(energyReward, energyPoolSlow);
            Vector3 validWorldPosittion = GetValidPosition(objectEcoClicker);
            MakeFX(energyPoolSlow, validWorldPosittion, fxCount, energyReward);   
        }

        public static void MakeFXOneFast(IObjectEcoClicker objectEcoClicker, BigNumber energyReward) {
            if (energyPoolFast.GetFreeElementsCount() == 0)
                throw new Exception("There is no free elements in the pool FAST");
            
            Vector3 validWorldPosittion = GetValidPosition(objectEcoClicker);
            MakeFX(energyPoolFast, validWorldPosittion, 1, energyReward); 
        }
        
        public static void MakeFXOneSlow(IObjectEcoClicker objectEcoClicker, BigNumber energyReward) {
            if (energyPoolSlow.GetFreeElementsCount() == 0)
                throw new Exception("There is no free elements in the pool FAST");
            
            Vector3 validWorldPosittion = GetValidPosition(objectEcoClicker);
            MakeFX(energyPoolSlow, validWorldPosittion, 1, energyReward); 
        }
        
        
        private IEnumerator CreatePoolOfFXs(Pool<FXCleanEnergy> pool, Vector3 validWoldPosition, int fxCount, BigNumber reward) {
            BigNumber rewardAverage = reward / fxCount;
            BigNumber rewardAverageCeil = reward / fxCount + 1;
            bool dividedEqually = reward % fxCount == 0;
            
            BigNumber rewardSum = reward;
            float timeBetweenFXs = FX_APPEARING_DURATION / fxCount;
            WaitForSeconds frame = new WaitForSeconds(timeBetweenFXs);

            for (int i = 0; i < fxCount; i++) {
                BigNumber rewardRewardRandom = dividedEqually ? rewardAverage : rewardAverageCeil;
                if (rewardRewardRandom > rewardSum)
                    rewardRewardRandom = rewardSum;
                rewardSum -= rewardRewardRandom;
                
                Vector3 position = GetRandomPositionAround(validWoldPosition, OFFSET);
                
                CreateFX(pool, position, rewardRewardRandom);
                yield return frame;
            }
        }
        
        private void CreateFX(Pool<FXCleanEnergy> pool, Vector3 worldPosition, BigNumber reward) {
            FXCleanEnergy fx = pool.GetFreeElement();
            fx.SetStartPosition(worldPosition);
            fx.SetReward(reward);
        }
        
        private void OnDestroy() {
            PanelCollectedCurrencyLeaf.OnLeafCollected -= OnLeafCollected;    
        }
    }
}