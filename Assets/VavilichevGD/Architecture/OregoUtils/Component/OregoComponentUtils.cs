using System;
using UnityEngine;

namespace Orego.Util
{
    public static class OregoComponentUtils
    {
        /**
         * Get.
         */

        public static T Get<T>(this Component component)
        {
            if (component == null)
            {
                throw new NullReferenceException("Component is not found!");
            }

            return component.GetComponent<T>();
        }

        public static T[] All<T>(this Component componenet)
        {
            return componenet.GetComponents<T>();
        }

        /**
         * Set.
         */

        public static T Add<T>(this Component component) where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }

        public static Component Add(this Component component, Type type)
        {
            return component.gameObject.AddComponent(type);
        }

        /**
         * Contains.
         */

        public static bool Has<T>(this Component component)
        {
            return component.GetComponent<T>() != null;
        }

        /*
         * Children.
         */

        public static T Child<T>(this Component component)
        {
            return component.GetComponentInChildren<T>(true);
        }

        public static T[] Children<T>(this Component component)
        {
            return component.GetComponentsInChildren<T>(true);
        }

        /**
         * Parent.
         */

        public static T Parent<T>(this Component component) where T : Component
        {
            return component.GetComponentInParent<T>();
        }

        public static T[] Parents<T>(this Component component) where T : Component
        {
            return component.GetComponentsInParent<T>(true);
        }

        public static void ClearParent(this Component component)
        {
            component.transform.SetParent(null);
        }

        public static void SetParent(this Component component, Component other)
        {
            component.transform.SetParent(other.transform);
        }

        public static void SetParent(this Component component, GameObject other)
        {
            component.transform.SetParent(other.transform);
        }
        
        
        public static void SetChild(this Component component, Component child)
        {
            child.SetParent(component);
        }

        public static void SetChild(this Component component, GameObject child)
        {
            child.SetParent(component);
        }


        /**
         * Visibility.
         */

        public static void SetInvisible(this Component component)
        {
            component.gameObject.SetActive(false);
        }

        public static void SetVisible(this Component component)
        {
            component.gameObject.SetActive(true);
        }

        /**
         * Transform.
         */

        public static void SetPosition(this Component component, Vector3 position)
        {
            component.transform.position = position;
        }

        public static void SetPosition(this Component component, float x, float y, float z)
        {
            component.SetPosition(new Vector3(x, y, z));
        }

        public static void SetLocalPosition(this Component component, Vector3 position)
        {
            component.transform.localPosition = position;
        }

        public static void SetRotation(this Component component, Quaternion rotation)
        {
            component.transform.rotation = rotation;
        }

        public static Vector3 GetPosition(this Component component)
        {
            return component.transform.position;
        }

        public static void ResetLocalPosition(this Component component)
        {
            component.transform.localPosition = Vector3.zero;
        }

        public static void SetEulerRotation(this Component component, Vector3 eulerAngle)
        {
            component.transform.rotation = Quaternion.Euler(eulerAngle);
        }
        
        public static void SetLocalEulerRotation(this Component component, Vector3 eulerAngle)
        {
            component.transform.localRotation = Quaternion.Euler(eulerAngle);
        }

        public static Vector3 GetEulerRotation(this Component component)
        {
            return component.transform.eulerAngles;
        }

        public static Quaternion GetRotation(this Component component)
        {
            return component.transform.rotation;
        }
        
        /**
         * Local scale.
         */

        public static Vector3 GetLocalScale(this Component component)
        {
            return component.transform.localScale;
        }
    }
}