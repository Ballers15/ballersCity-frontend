using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orego.Util
{
    public static class OregoRaycastUtils
    {
        #region 2D

        public static bool HasCollider2DByRaycast(
            this MonoBehaviour behaviour,
            Vector2 direction,
            float distance,
            Func<Collider2D, bool> condition
        )
        {
            return Physics2D
                .RaycastAll(behaviour.transform.position, direction, distance)
                .Any(raycastHit2D => condition(raycastHit2D.collider));
        }


        public static List<T> FindComponentsByRaycast2D<T>(
            this MonoBehaviour behaviour,
            Vector2 direction,
            float distance,
            Func<Collider2D, bool> condition
        ) where T : Component
        {
            return Physics2D
                .RaycastAll(behaviour.transform.position, direction, distance)
                .Select(raycastHit2D => raycastHit2D.collider)
                .Where(condition)
                .Select(otherCollider => otherCollider.gameObject)
                .Select(otherGameObject => otherGameObject.GetComponent<T>())
                .Where(requiredComponent => requiredComponent != null)
                .ToList();
        }

        #endregion

        #region 3D

        public static bool HasColliderByRaycast(
            this MonoBehaviour behaviour,
            Vector3 direction,
            float distance,
            Func<Collider, bool> condition
        )
        {
            return Physics
                .RaycastAll(behaviour.transform.position, direction, distance)
                .Any(raycastHit => condition(raycastHit.collider));
        }


        public static List<T> FindComponentsByRaycast<T>(
            this MonoBehaviour behaviour,
            Vector3 direction,
            float distance,
            Func<Collider, bool> condition
        ) where T : Component
        {
            return Physics
                .RaycastAll(behaviour.transform.position, direction, distance)
                .Select(raycastHit => raycastHit.collider)
                .Where(condition)
                .Select(otherCollider => otherCollider.gameObject)
                .Select(otherGameObject => otherGameObject.GetComponent<T>())
                .Where(requiredComponent => requiredComponent != null)
                .ToList();
        }

        #endregion
    }
}