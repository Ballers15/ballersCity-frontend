using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Core
{
    public sealed class IdleObjectsInteractor : Interactor, IUpdateListenerInteractor, ISaveListenerInteractor {
        
        private IdleObjectsRepository idleObjectsRepository;

        private readonly Dictionary<string, IdleObject> idleObjectsMap;

        public IdleObjectsInteractor() {
            this.idleObjectsMap = new Dictionary<string, IdleObject>();
        }

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            IdleObject.OnIdleObjectBuilt += this.IdleObjectNewOnIdleObjectBuilt;
            this.idleObjectsRepository = this.GetRepository<IdleObjectsRepository>();
            
            var states = idleObjectsRepository.states;
            var idleObjects = Object.FindObjectsOfType<IdleObject>();
            foreach (var idleObject in idleObjects)
            {
                var stateFounded = false;
                foreach (var stateJson in states.stateJsons)
                {
                    var state = new IdleObjectState(stateJson);
                    if (state.id == idleObject.id)
                    {
                        idleObject.Initialize(stateJson);
                        stateFounded = true;
                        break;
                    }
                }

                if (!stateFounded)
                {
                    idleObject.Initialize();
                }

                this.idleObjectsMap.Add(idleObject.id, idleObject);
            }
        }
        
        public void OnUpdate(float unscaledDeltaTime)
        {
            var idleObjects = this.idleObjectsMap.Values;
            foreach (var idleObject in idleObjects)
            {
                idleObject.ForceUpdate(unscaledDeltaTime);
            }
        }

        public void OnSave()
        {
            var idleObjectStates = new List<IdleObjectState>();
            var idleObjects = this.idleObjectsMap.Values;
            foreach (var idleObject in idleObjects)
            {
                idleObjectStates.Add(idleObject.state);
            }

            this.idleObjectsRepository.Save(idleObjectStates.ToArray());
        }

        public IdleObject[] GetIdleObjects()
        {
            return idleObjectsMap.Values.ToArray();
        }

        public IdleObject[] GetBuildedIdleObjects()
        {
            List<IdleObject> listBuildedIdle = new List<IdleObject>();
            foreach(var idle in idleObjectsMap.Values)
            {
                if (idle.isBuilt)
                    listBuildedIdle.Add(idle);
            }
            return listBuildedIdle.ToArray();
        }

        public IdleObject GetIdleObject(string id)
        {
            if (idleObjectsMap.ContainsKey(id))
            {
                return idleObjectsMap[id];
            }

            throw new Exception("Idle Object not found!");
        }

        public IdleObject GetLastBuiltObject() {
            var idleObjects = this.idleObjectsMap.Values.ToArray();
            Array.Sort(idleObjects,
                delegate(IdleObject x, IdleObject y) {
                    return x.info.number.CompareTo(y.info.number);
                });
            
            
            for (int i = 1; i < idleObjects.Length; i++) {
                if (!idleObjects[i].isBuilt)
                    return idleObjects[i - 1];
            }

            return null;
        }

        #region Events
        
        private void IdleObjectNewOnIdleObjectBuilt(IdleObject idleobject, IdleObjectState newstate)
        {
            this.OnSave();
        }
        #endregion
    }
}