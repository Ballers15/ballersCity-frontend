using Ediiie.Model;
using System;
using UnityEngine;

namespace Notification
{
    public abstract class Notification : MonoBehaviour
    {
        protected virtual void Awake()
        {
         
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void OnInit(Ediiie.Model.STATUS status, string message);
    }
}