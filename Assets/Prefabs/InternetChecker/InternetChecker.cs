using System;
using System.Collections;
using UnityEngine;

namespace SinSity.Utils {
    public class InternetChecker : MonoBehaviour {
#if !UNITY_WEBGL

        #region CONSTANTS

        private const string ADRESS = "8.8.8.8"; // Google Public DNS server
        private const float PERIOD = 5.0f;

        #endregion
       
        public static bool internetConnected { get; private set; }
        

        private void OnEnable() {
            bool internetPossiblyAvailable;
            
            switch (Application.internetReachability) {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetPossiblyAvailable = true;
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetPossiblyAvailable = true;
                    break;
                default:
                    internetPossiblyAvailable = false;
                    break;
            }

            if (!internetPossiblyAvailable) {
                this.InternetIsNotAvailable();
                return;
            }

            this.StartCoroutine("CheckRoutine");
        }

        private void OnDisable() {
            this.StopCoroutine("CheckRoutine");
        }


        private IEnumerator CheckRoutine() {
            float timer = 0f;
            var oneSecFrame = new WaitForSecondsRealtime(1f);


            while (true) {
                var ping = new Ping(ADRESS);
                
                if (ping.isDone)
                    this.InternetAvailable();
                else {
                    while (!ping.isDone) {
                        yield return oneSecFrame;
                        
                        if (ping.isDone) {
                            this.InternetAvailable();
                            break;
                        }

                        timer += 1f;

                        if (timer >= 3) {
                            ping.DestroyPing();
                            this.InternetIsNotAvailable();
                            break;
                        }
                    }
                }

                yield return new WaitForSecondsRealtime(PERIOD);
            }
        }

        public void InternetIsNotAvailable() {
            Debug.Log("No Internet");
            internetConnected = false;
        }

        public void InternetAvailable() {
            Debug.Log("Internet is available;)");
            internetConnected = true;
        }
#endif
    }
}