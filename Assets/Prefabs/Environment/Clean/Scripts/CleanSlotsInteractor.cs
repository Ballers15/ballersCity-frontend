using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using SinSity.Domain;
using SinSity.Scripts;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Core {
    public sealed class CleanSlotsInteractor : Interactor {
        #region Const

        private const string CLEAN_SLOT_PREFABS_FOLDER = "CleanPrefabs/";

        #endregion

        private readonly HashSet<string> cleanSlotsNamesSetNotBuilded;

        private readonly Dictionary<string, CleanSlot> idleObjectIdVsCleanSlotsMap;

        private Dictionary<string, string> idleObjectIdVsCleanSlotsNamesMap;

        private Dictionary<string, Zone> zoneIdMap;

        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        public CleanSlotsInteractor() {
            IdleObject.OnIdleObjectBuilt += OnIdleObjectBuilt;
            this.cleanSlotsNamesSetNotBuilded = new HashSet<string>();
            this.idleObjectIdVsCleanSlotsMap = new Dictionary<string, CleanSlot>();
        }


        protected override IEnumerator InitializeRoutineNew() {
            this.zoneIdMap = Object
                .FindObjectsOfType<Zone>()
                .ToDictionary(it => it.name);
            this.idleObjectIdVsCleanSlotsNamesMap = Resources
                .LoadAll<CleanSlot>(CLEAN_SLOT_PREFABS_FOLDER)
                .ToDictionary(it => it.idleObjectId, it => it.name);
            yield return null;
            this.CreateCleanSlotsIfNecessary();
            Resources.UnloadUnusedAssets();
            yield return null;
        }

        private void CreateCleanSlotsIfNecessary() {
            var idleObjects = this.GetAllIdleObjects();
            foreach (var idleObject in idleObjects) {
                var idleObjectId = idleObject.id;
                if (this.idleObjectIdVsCleanSlotsNamesMap.ContainsKey(idleObjectId)) {
                    var createdSlot = this.CreateCleanSlot(idleObjectId);
                    this.idleObjectIdVsCleanSlotsMap[idleObjectId] = createdSlot;

                    if (idleObject.isBuildedAnyTime)
                        createdSlot.Activate(true);
                    else
                        createdSlot.Deactivate();
//                    createdSlot.ShowCreatingWithoutAnimations();
                }

                //}
//                else if (!idleObject.isBuilt && this.idleObjectIdVsCleanSlotsNamesMap.ContainsKey(idleObjectId)) {
//                    this.cleanSlotsNamesSetNotBuilded.Add(idleObjectId);
//                }
            }

            Resources.UnloadUnusedAssets();
        }

        private IEnumerable<IdleObject> GetAllIdleObjects() {
            var idleObjectNewList = new List<IdleObject>();
            var zones = this.zoneIdMap.Values;
            foreach (var zone in zones) {
                idleObjectNewList.AddRange(zone.idleObjects);
            }

            return idleObjectNewList;
        }

        #endregion

        #region OnIdleObjectBuilt

        private void OnIdleObjectBuilt(IdleObject idleObject, IdleObjectState state) {
            if (!this.idleObjectIdVsCleanSlotsMap.ContainsKey(idleObject.id))
                return;
            
            var idleObjectId = idleObject.id;
            var cleanSlot = this.idleObjectIdVsCleanSlotsMap[idleObjectId];
            if (!cleanSlot.isActive)
                cleanSlot.Activate();
//            this.CreateCleanSlotInGame(idleObjectId);
        }

        private void CreateCleanSlotInGame(string idleObjectId) {
            if (!this.cleanSlotsNamesSetNotBuilded.Contains(idleObjectId))
                return;

            if (!this.idleObjectIdVsCleanSlotsNamesMap.ContainsKey(idleObjectId)) {
                return;
            }

            var createdSlot = this.CreateCleanSlot(idleObjectId);
            createdSlot.ShowCreatingWithAnimation();
            this.cleanSlotsNamesSetNotBuilded.Remove(idleObjectId);
            if (this.cleanSlotsNamesSetNotBuilded.Count == 0) {
                IdleObject.OnIdleObjectBuilt -= OnIdleObjectBuilt;
            }
        }

        #endregion

        #region OnIdleObjectReset

        public IEnumerator OnIdleObjectReset(IdleObject idleObject) {
            var idleObjectId = idleObject.id;
            if (!this.idleObjectIdVsCleanSlotsMap.ContainsKey(idleObjectId)) {
                yield break;
            }

//            yield return this.DestroyCleanSlot(idleObjectId);
            var cleanSlot = this.idleObjectIdVsCleanSlotsMap[idleObjectId];
            cleanSlot.Deactivate();
        }

        #endregion

        public CleanSlot CreateCleanSlot(string idleObjectId) {
            if (this.idleObjectIdVsCleanSlotsMap.ContainsKey(idleObjectId))
                return this.idleObjectIdVsCleanSlotsMap[idleObjectId];

            var cleanSlotsName = this.idleObjectIdVsCleanSlotsNamesMap[idleObjectId];
            var cleanSlotPref = Resources.Load<CleanSlot>($"{CLEAN_SLOT_PREFABS_FOLDER}{cleanSlotsName}");
            var containerName = cleanSlotPref.zoneName;
            var zone = this.zoneIdMap[containerName];
            var container = zone.cleanContainer;
            var createdSlot = Object.Instantiate(cleanSlotPref, container);
            this.idleObjectIdVsCleanSlotsMap.Add(idleObjectId, createdSlot);
            return createdSlot;
        }
        
        

        public IEnumerator DestroyCleanSlot(string idleObjectId) {
            var cleanSlot = this.idleObjectIdVsCleanSlotsMap[idleObjectId];
            cleanSlot.Deactivate();
            yield return cleanSlot.DestroyWithoutAnimations();
            this.idleObjectIdVsCleanSlotsMap.Remove(idleObjectId);
        }

        public void Reset() {
            cleanSlotsNamesSetNotBuilded.Clear();
            var idleObjects = this.GetAllIdleObjects();
            foreach (var idleObject in idleObjects) {
                var idleObjectId = idleObject.id;
                this.cleanSlotsNamesSetNotBuilded.Add(idleObjectId);
            }
        }
    }
}