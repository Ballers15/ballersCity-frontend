using UnityEngine;

namespace Orego.Util
{
    public static class OregoComponentVectorUtils
    {
        /**
         * Distance.
         */

        public static Vector3 GetDistanceTo(this Component component, GameObject other)
        {
            return other.GetPosition() - component.GetPosition();
        }

        public static Vector3 GetDistanceTo(this Component component, Component other)
        {
            return other.GetPosition() - component.GetPosition();
        }

        public static float GetMagnitudeTo(this Component component, GameObject other)
        {
            return component.GetDistanceTo(other).magnitude;
        }

        public static float GetMagnitudeTo(this Component component, Component other)
        {
            return component.GetDistanceTo(other).magnitude;
        }
    }
}