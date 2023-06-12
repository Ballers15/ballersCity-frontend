using System.Collections;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SinSity.Meta {
    public class FortuneWheelRepository : Repository {

        #region CONSTANTS

        private const string DATA_KEY = "FORTUNE_WHEEL_DATA";
        private const string CONFIG_PATH = "Config/FortuneWheelConfig";

        #endregion
        
        public FortuneWheelData data { get; private set; }
        public FortuneWheelConfig config { get; private set; }
        
        public override void OnCreate() {
            base.OnCreate();
            this.config = Resources.Load<FortuneWheelConfig>(CONFIG_PATH);
            
            if (!GameTime.isInitialized)
                GameTime.OnGameTimeInitialized += this.OnGameTimeInitialized;
            else
                this.InitData();
        }

        private void OnGameTimeInitialized() {
            GameTime.OnGameTimeInitialized -= this.OnGameTimeInitialized;
            this.InitData();
        }

        private void InitData() {
            if (!Storage.HasObject(DATA_KEY)) {
                var firstPlayTime = GameTime.now;                            // WARNING: TODO: You must know that game time is initialized;
                var gemPriceDefault = this.config.gemPriceSpinDefault;
                this.data = new FortuneWheelData(gemPriceDefault, firstPlayTime);
            }

            this.data = Storage.GetCustom(DATA_KEY, this.data);
        }

        public override void Save() {
            Storage.SetCustom(DATA_KEY, this.data);
        }

        
        #if UNITY_EDITOR
        [MenuItem("Database/Fortune Wheel/Clear all data")]
        public static void ClearAllData() {
            Storage.ClearKey(DATA_KEY);
        }
        #endif
    }
}