#if UNITY_EDITOR
using System;
using EcoClickerScripts.Tools;
using UnityEngine;
using VavilichevGD.Tools;

namespace IceBreakers.Tools
{
    public sealed class CheatCodesController : MonoBehaviour {

        private Cheats cheats;
        
        [RuntimeInitializeOnLoadMethod]
        public static void Create()
        {
            var o = new GameObject("CheatCodes");
            var instance = o.AddComponent<CheatCodesController>();
            instance.cheats = new Cheats();
            DontDestroyOnLoad(o);
        }

        private void FixedUpdate() {

            OnKeyUp(KeyCode.G,
                () => this.cheats.AddCleanEnergy(
                    new BigNumber("10000000000000000000000000000000000000000000000000000000")));
            OnKeyUp(KeyCode.H, () => this.cheats.AddGems(1000));
            OnKeyUp(KeyCode.E, () => this.cheats.AddExperience());
            
        }

        private static void OnKeyUp(KeyCode keyCode, Action action)
        {
            if (Input.GetKeyUp(keyCode))
            {
                action?.Invoke();
            }
        }

    }
}
#endif