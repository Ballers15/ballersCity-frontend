using System;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Scripts {
    public abstract class InfoLoader : IDisposable {
        public abstract string infosPath { get; }

        protected IEnumerable<T> LoadAs<T>() where T : UnityEngine.Object {
            return Resources.LoadAll<T>(infosPath);
        }
        
        public void Dispose() {
            Resources.UnloadUnusedAssets();
        }
    }
}