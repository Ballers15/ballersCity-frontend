using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Domain
{
    public sealed class HintSystemInteractor : Interactor
    {
        #region Const

        private const string HINT_INSPECTOR_FOLDER = "HintInspectors";

        #endregion

        #region Event

        public event Action<HintInspector> OnInspectorStateChangedEvent;

        public event Action<HintInspector> OnHintInspectorTriggeredEvent;

        #endregion

        private Dictionary<Type, HintInspector> inspectorMap;
        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.inspectorMap = Resources
                .LoadAll<HintInspector>(HINT_INSPECTOR_FOLDER)
                .Select(Object.Instantiate)
                .ToDictionary(it => it.GetType());
            this.InitializeInspectors();
        }


        private void InitializeInspectors()
        {
            var inspectors = this.inspectorMap.Values;
            foreach (var inspector in inspectors)
            {
                inspector.OnInitialize();
                inspector.OnStateChangedEvent += this.OnInspectorStateChanged;
                inspector.OnTriggeredEvent += this.OnInspectorTriggered;
            }
        }

        #endregion

        #region OnReady

        public override void OnReady()
        {
            base.OnReady();
            var inspectors = this.inspectorMap.Values;
            foreach (var inspector in inspectors)
                inspector.OnReady();
            
            foreach (var inspector in inspectors)
                inspector.OnStart();
        }


        #endregion

        
        public T GetInspector<T>() where T : HintInspector
        {
            return (T) this.inspectorMap[typeof(T)];
        }

        #region Events

        private void OnInspectorStateChanged(HintInspector inspector)
        {
            this.OnInspectorStateChangedEvent?.Invoke(inspector);
        }

        private void OnInspectorTriggered(HintInspector inspector)
        {
            this.OnHintInspectorTriggeredEvent?.Invoke(inspector);
        }

        #endregion
    }
}