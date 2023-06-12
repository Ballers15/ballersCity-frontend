using System.Collections;
//using Firebase;
//using Firebase.Analytics;
using Orego.Util;
using SinSity.Scripts;
using UnityEngine;

namespace SinSity.Domain
{
    public sealed class FirebaseLauncher : IBootLauncher
    {
        public IEnumerator Launch()
        {
            /*yield return Continuation.Suspend(continuation => FirebaseApp
                .CheckAndFixDependenciesAsync()
                .ContinueWith(task =>
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    continuation.Continue();
                }));*/
            yield break;
        }
    }
}