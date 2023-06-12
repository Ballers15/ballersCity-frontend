using System.Collections;
//using Facebook.Unity;
using Orego.Util;
using SinSity.Scripts;

namespace SinSity.Domain
{
    public sealed class FacebookLauncher : IBootLauncher
    {
        public IEnumerator Launch()
        {
            //HERE WAS FACEBOOK SDK
            /*if (FB.IsInitialized)
            {
                FB.ActivateApp();
                yield break;
            }
            
            yield return Continuation.Suspend(continuation => FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }

                continuation.Continue();
            }));*/
            yield break;
        }
    }
}