using System.Collections;
using UnityEngine;

namespace Orego.Util
{
    public static class OregoParticleSystemUtils
    {
        public static IEnumerator ShowOneShot(this ParticleSystem particleSystem)
        {
            particleSystem.Play();
            yield return new WaitForSeconds(particleSystem.main.duration);
            particleSystem.Stop();
        }
    }
}