using UnityEngine;

namespace Orego.Util
{
    public static class OregoCollisionUtils
    {
        public static void DisableCollision(this Component component, GameObject other)
        {
            component.gameObject.DisableCollision(other);
        }

        public static void DisableCollision(this GameObject gameObject, GameObject other)
        {
            gameObject.IgnoreCollision(other, true);
        }

        public static void EnableCollision(this GameObject gameObject, GameObject other)
        {
            gameObject.IgnoreCollision(other, false);
        }

        public static void IgnoreCollision(this Component component, GameObject other, bool isIgnore)
        {
            component.gameObject.IgnoreCollision(other, isIgnore);
        }

        public static void IgnoreCollision(this GameObject gameObject, GameObject other, bool isIgnore)
        {
            var colliders = gameObject.All<Collider>();
            var otherColliders = other.All<Collider>();
            foreach (var collider in colliders)
            {
                foreach (var otherCollider in otherColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, isIgnore);
                }
            }
        }
    }
}