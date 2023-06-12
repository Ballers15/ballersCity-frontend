using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orego.Util
{
    public abstract class AutoMonoBehaviour : MonoBehaviour, IDisposable
    {
        private readonly HashSet<ScriptableObject> scriptableObjects = new HashSet<ScriptableObject>();

        private readonly HashSet<IAutoEvent> events = new HashSet<IAutoEvent>();

        protected T AutoInstantiate<T>(T asset) where T : ScriptableObject
        {
            var scriptableObject = Instantiate(asset);
            this.scriptableObjects.Add(scriptableObject);
            return scriptableObject;
        }

        protected T AutoInstantiate<T>() where T : IAutoEvent, new()
        {
            var newEvent = new T();
            this.events.Add(newEvent);
            return newEvent;
        }

        protected virtual void OnDestroy()
        {
            this.Dispose();
        }

        public virtual void Dispose()
        {
            this.scriptableObjects.ForEach(Destroy);
            this.scriptableObjects.Clear();
            this.events.ForEach(it => it.Dispose());
            this.events.Clear();
        }
    }
}