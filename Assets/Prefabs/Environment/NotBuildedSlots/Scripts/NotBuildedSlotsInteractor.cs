using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Core
{
    public sealed class NotBuildedSlotsInteractor : Interactor
    {
        #region Const

        private const string NOT_BUILDED_SLOT_PREFABS_FOLDER = "NotBuildedSlotPrefabs/";

        #endregion

        private readonly Dictionary<string, NotBuildedSlot> idleObjectIdsCreatedNotBuildedSlotsMap;

        private Dictionary<string, string> idleObjectIdsDirtNamesMap;
        private Dictionary<string, string> idleObjectIdsFencesNamesMap;
        private Dictionary<string, Zone> zoneIdMap;

        #region Initialize

        public NotBuildedSlotsInteractor() {
            this.idleObjectIdsCreatedNotBuildedSlotsMap = new Dictionary<string, NotBuildedSlot>();
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
        }

        public override bool onCreateInstantly { get; }

        protected override IEnumerator InitializeRoutineNew() {
            this.idleObjectIdsDirtNamesMap = Resources
                .LoadAll<Dirt>(NOT_BUILDED_SLOT_PREFABS_FOLDER)
                .ToDictionary(it => it.idleObjectId, it => it.name);

            this.idleObjectIdsFencesNamesMap = Resources
                .LoadAll<Fence>(NOT_BUILDED_SLOT_PREFABS_FOLDER)
                .ToDictionary(it => it.idleObjectId, it => it.name);

            this.zoneIdMap = Object
                .FindObjectsOfType<Zone>()
                .ToDictionary(it => it.name);

            yield return null;
            yield return this.CreateNotBuildedSlotsIfNecessary();
        }

        private IEnumerator CreateNotBuildedSlotsIfNecessary()
        {
            var idleObjectsNew = this.GetAllIdleObjectsFromZones();
            foreach (var idleObject in idleObjectsNew)
            {
                var idleObjectId = idleObject.id;

                if (this.idleObjectIdsDirtNamesMap.ContainsKey(idleObjectId) && !idleObject.isBuilt)
                {
                    if (idleObject.isBuildedAnyTime)
                    {
                        yield return this.CreateFence(idleObjectId);
                    }
                    else
                    {
                        yield return this.CreateDirt(idleObjectId);
                    }
                }
            }

            Resources.UnloadUnusedAssets();
        }

        private IEnumerable<IdleObject> GetAllIdleObjectsFromZones()
        {
            var idleObjectNewList = new List<IdleObject>();
            var zones = this.zoneIdMap.Values;
            foreach (var zone in zones)
                idleObjectNewList.AddRange(zone.idleObjects);

            return idleObjectNewList;
        }

        #endregion

        #region OnIdleObjectBuilt

        private void OnIdleObjectBuilt(IdleObject idleObject, IdleObjectState state)
        {
            var idleObjectId = idleObject.id;
            this.DestroyNotBuildedSlot(idleObjectId);
        }

        #endregion

        #region OnIdleObjectReset

        public IEnumerator OnIdleObjectReset(IdleObject idleObject)
        {
            var idleObjectId = idleObject.id;
            if (this.idleObjectIdsCreatedNotBuildedSlotsMap.ContainsKey(idleObjectId))
            {
                yield break;
            }

            if (idleObject.isBuildedAnyTime)
            {
                yield return this.CreateFence(idleObjectId);
            }
            else
            {
                yield return this.CreateDirt(idleObjectId);
            }
        }

        #endregion

        #region CreateDirt

        public IEnumerator CreateDirt(string idleObjectId)
        {
            var dirtRef = new Reference<Dirt>();
            yield return CreateNotBuildedSlot(idleObjectId, idleObjectIdsDirtNamesMap, dirtRef);
            this.idleObjectIdsCreatedNotBuildedSlotsMap.Add(idleObjectId, dirtRef.value);
        }

        public IEnumerator CreateFence(string idleObjectId)
        {
            var fenceRef = new Reference<Fence>();
            yield return CreateNotBuildedSlot(idleObjectId, idleObjectIdsFencesNamesMap, fenceRef);
            this.idleObjectIdsCreatedNotBuildedSlotsMap.Add(idleObjectId, fenceRef.value);
        }

        private IEnumerator CreateNotBuildedSlot<T>(
            string idleObjectId,
            Dictionary<string, string> namesMap,
            Reference<T> result
        ) where T : NotBuildedSlot
        {
            var prefName = namesMap[idleObjectId];
            var resourceRequest = Resources.LoadAsync<T>($"{NOT_BUILDED_SLOT_PREFABS_FOLDER}{prefName}");
            yield return resourceRequest;
            var slotPref = (T) resourceRequest.asset;
            var containerName = slotPref.zoneName;
            var zone = this.zoneIdMap[containerName];
            var transformContainer = zone.notBuildedSlotContainer;
            var createdSlot = Object.Instantiate(slotPref, transformContainer);
            result.value = createdSlot;
            yield return new WaitForEndOfFrame();
        }

        #endregion

        #region DestroyNotBuildedSlot

        public void DestroyNotBuildedSlot(string idleObjectId)
        {
            if (!this.idleObjectIdsCreatedNotBuildedSlotsMap.ContainsKey(idleObjectId))
                return;

            NotBuildedSlot notBuildedSlot = this.idleObjectIdsCreatedNotBuildedSlotsMap[idleObjectId];
            notBuildedSlot.DestroyWithAnimation();
            this.idleObjectIdsCreatedNotBuildedSlotsMap.Remove(idleObjectId);
        }

        #endregion
    }
}