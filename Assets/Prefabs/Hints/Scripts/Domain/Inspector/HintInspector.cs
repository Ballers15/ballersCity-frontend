using System;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public abstract class HintInspector : ScriptableObject
    {
        #region Event

        public event Action<HintInspector> OnStateChangedEvent;

        public event Action<HintInspector> OnTriggeredEvent;

        #endregion

        [SerializeField]
        private string m_hintId;

        public string hintId
        {
            get { return this.m_hintId; }
        }

        protected HintRepository repository { get; private set; }

        #region OnInitialized

        public void OnInitialize()
        {
            this.repository = Game.GetRepository<HintRepository>();
            var json = this.repository.GetState(this.hintId);
            this.OnInitState(json);
        }

        protected abstract void OnInitState(string hintValue);

        #endregion

        public virtual void OnReady()
        {
        }

        public virtual void OnStart()
        {
            
        }

        protected void NotifyAboutStateChanged()
        {
            this.OnStateChangedEvent?.Invoke(this);
        }

        protected void NotifyAboutTriggered()
        {
            this.OnTriggeredEvent?.Invoke(this);
        }
    }
}