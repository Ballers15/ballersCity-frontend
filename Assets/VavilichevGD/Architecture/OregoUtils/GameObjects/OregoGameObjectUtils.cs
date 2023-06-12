using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orego.Util
{
    public static class OregoGameObjectUtils
    {
        /**
         * Transform.
         */

        public static void SetTransform(this GameObject myObject, GameObject original)
        {
            if (original != null)
            {
                var transform = myObject.transform;
                var originalTransform = original.transform;
                transform.position = originalTransform.position;
                transform.rotation = originalTransform.rotation;
            }
        }

        /**
         * Transform.
         */
        
        public static void SetPosition(this GameObject myObject, Vector3 position)
        {
            var transform = myObject.transform;
            transform.position = position;
        }

        public static Vector3 GetPosition(this GameObject gameObject)
        {
            return gameObject.transform.position;
        }
        
        public static void SetRotation(this GameObject gameObject, Quaternion rotation)
        {
            gameObject.transform.rotation = rotation;
        }


        public static Vector3 GetLocalPosition(this GameObject gameObject)
        {
            return gameObject.transform.localPosition;
        }
        
        public static Quaternion GetRotation(this GameObject gameObject)
        {
            return gameObject.transform.rotation;
        }
        
        public static Vector3 GetEulerRotation(this GameObject gameObject)
        {
            return gameObject.transform.rotation.eulerAngles;
        }

        /**
         * Has.
         */

        public static bool Has<T>(this GameObject myObject)
        {
            return myObject.GetComponent<T>() != null;
        }
        
        public static bool Has<T>(this GameObject component, out T result)
        {
            var requiredComponent = component.GetComponent<T>();
            if (requiredComponent != null)
            {
                result = requiredComponent;
                return true;
            }

            result = default(T);
            return false;
        }

        public static bool Has(this GameObject myObject, Type type)
        {
            return myObject.GetComponent(type) != null;
        }

        /**
         * Parent.
         */

        public static void SetParent(this GameObject gameObject, Component other)
        {
            gameObject.transform.SetParent(other.transform);
        }

        /**
         * Get.
         */

        public static T Get<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                throw new NullReferenceException("Component is not found!");
            }

            return component;
        }

        public static T[] All<T>(this GameObject gameObject)
        {
            return gameObject.GetComponents<T>();
        }

        /**
         * Children.
         */

        public static bool HasChild<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.Child<T>() != null;
        }

        public static T[] Children<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponentsInChildren<T>();
        }

        public static T Child<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponentInChildren<T>(true);
        }
        
        public static void SetChild(this GameObject gameObject, GameObject other)
        {
            other.transform.SetParent(gameObject.transform);
        }

        /**
         * Set.
         */

        public static T Add<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.AddComponent<T>();
        }

        public static Component Add(this GameObject gameObject, Type type)
        {
            return gameObject.AddComponent(type);
        }

        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public static void DestroyItSelf(this GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
        
        public static void SetEulerRotation(this GameObject gameObject, Vector3 eulerAngle)
        {
            gameObject.transform.rotation = Quaternion.Euler(eulerAngle);
        }
    }
}