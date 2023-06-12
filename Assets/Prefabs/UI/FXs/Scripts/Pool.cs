using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Tools {
    public class Pool<T> where  T : MonoBehaviour {

        protected List<T> pool;
        
        public Pool(T prefab, int count) {
            CreatePool(prefab, count, null);
        }

        private void CreatePool(T prefab, int count, Transform transformContainer) {
            pool = new List<T>();

            for (int i = 0; i < count; i++) {
                T obj = Object.Instantiate(prefab, transformContainer);
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }
        
        public Pool(T prefab, int count, Transform transformContainer) {
            CreatePool(prefab, count, transformContainer);
        }

        public T GetFreeElement() {
            foreach (T monoBehaviour in pool) {
                if (!monoBehaviour.gameObject.activeInHierarchy) {
                    monoBehaviour.gameObject.SetActive(true);
                    return monoBehaviour;
                }
            }

            throw new Exception($"The pool is empty. Current elements number is: {pool.Count}");
        }

        public T[] GetAllElements() {
            return pool.ToArray();
        }

        public int GetFreeElementsCount() {
            int sum = 0;
            foreach (T monoBehaviour in pool) {
                if (!monoBehaviour.gameObject.activeInHierarchy)
                    sum++;
            }

            return sum;
        }
    }
}