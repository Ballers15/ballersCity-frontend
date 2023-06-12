using System;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public abstract class TutorialStageController : ScriptableObject
    {
        #region Event

        public event Action<TutorialStageController> OnStateChangedEvent;

        public event Action<TutorialStageController> OnTriggeredEvent;

        #endregion

        [SerializeField]
        private string m_id;

        public string id
        {
            get { return this.m_id; }
        }

        protected TutorialRepository repository { get; private set; }

        public void OnInitialize()
        {
            this.repository = Game.GetRepository<TutorialRepository>();
        }

        protected void NotifyAboutStateChanged()
        {
            this.OnStateChangedEvent?.Invoke(this);
        }

        protected void NotifyAboutTriggered()
        {
            this.OnTriggeredEvent?.Invoke(this);
        }

        public virtual void OnBeginListen()
        {
        }

        public virtual void OnContinueListen(string currentStageJson)
        {
        }

        public virtual void OnEndListen()
        {
            
        }

        public virtual string GetState()
        {
            return null;
        }
    }
}