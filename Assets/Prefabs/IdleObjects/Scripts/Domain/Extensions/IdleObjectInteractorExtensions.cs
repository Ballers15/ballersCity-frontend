using System;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Domain.Utils
{
    public static class IdleObjectInteractorExtensions
    {
        public static IEnumerable<IdleObject> GetBuiltIdleObjects(this IdleObjectsInteractor idleObjectsInteractor)
        {
            var idleObjects = idleObjectsInteractor.GetIdleObjects();
            var builtIdleObjects = idleObjects.Where(it => it.isBuilt);
            return builtIdleObjects;
        }
        
        public static bool IdleObjectIsBuilt(this IdleObjectsInteractor idleObjectsInteractor, string idleId)
        {
            var idleObjects = idleObjectsInteractor.GetIdleObject(idleId);
            return idleObjects.isBuilt;
        }

        public static IEnumerable<IdleObject> GetNotBuiltIdleObjects(this IdleObjectsInteractor idleObjectsInteractor)
        {
            var idleObjects = idleObjectsInteractor.GetIdleObjects();
            var notBuiltIdleObjects = idleObjects.Where(it => !it.isBuilt);
            return notBuiltIdleObjects;
        }

        public static BigNumber GetFullIncomeFromIdleObjects(this IdleObjectsInteractor idleObjectsInteractor)
        {
            var builtIdleObjects = idleObjectsInteractor.GetBuiltIdleObjects();
            if (builtIdleObjects.IsEmpty())
            {
                return new BigNumber(0);
            }

            var fullIncome = new BigNumber(0);
            foreach (var idleObject in builtIdleObjects)
            {
                var incomeCurrent = idleObject.incomePerSec;
                fullIncome += incomeCurrent;
            }

            return fullIncome;
        }

        public static BigNumber GetFullIncomeByIdleObjectsForTime(this IdleObjectsInteractor idleObjectsInteractor,
            int seconds) {
            var builtIdleObjects = idleObjectsInteractor.GetBuiltIdleObjects();
            if (builtIdleObjects.IsEmpty())
                return new BigNumber(0);

            var fullIncome = new BigNumber(0);
            foreach (var idleObject in builtIdleObjects)
            {
                var incomeCurrent = idleObject.incomePerSec;
                if (idleObject.autoplayEnabled)
                    fullIncome += incomeCurrent * seconds;
                else
                    fullIncome += idleObject.incomePerSec * Mathf.Min(seconds, idleObject.state.incomePeriod);
            }

            return fullIncome;
        }

        public static bool HasMostExpensiveBuiltIdleObject(
            this IdleObjectsInteractor idleObjectsInteractor,
            out IdleObject idleObject
        )
        {
            var builtObjects = idleObjectsInteractor.GetBuiltIdleObjects();
            if (builtObjects.IsEmpty())
            {
                idleObject = null;
                return false;
            }

            var expensiveSortedObjects = builtObjects
                .OrderBy(builtObject => builtObject.info.priceToBuild.bigIntegerValue)
                .ToList();
            var mostCostObject = expensiveSortedObjects[expensiveSortedObjects.Count - 1];
            idleObject = mostCostObject;
            return idleObject;
        }

        public static bool HasMostCheapNotBuiltIdleObject(
            this IdleObjectsInteractor idleObjectsInteractor,
            out IdleObject idleObject
        )
        {
            var notBuiltObjects = idleObjectsInteractor.GetNotBuiltIdleObjects();
            var builtObjects = notBuiltObjects as IdleObject[] ?? notBuiltObjects.ToArray();
            if (builtObjects.IsEmpty())
            {
                idleObject = null;
                return false;
            }

            var cheapSortedObjects = builtObjects
                .OrderBy(notBuiltObject => notBuiltObject.info.priceToBuild.bigIntegerValue)
                .ToList();
            var mostCheapObject = cheapSortedObjects[0];
            idleObject = mostCheapObject;
            return true;
        }

        public static BigNumber GetAllCollectedIdleObjectCurrency(this IdleObjectsInteractor idleObjectsInteractor)
        {
            var allCollectedCurrency = new BigNumber(0);
            var builtIdleObjects = idleObjectsInteractor.GetBuiltIdleObjects();
            foreach (var builtIdleObject in builtIdleObjects)
            {
                var idleObjectState = builtIdleObject.state;
                var collectedCurrency = idleObjectState.collectedCurrency;
                allCollectedCurrency += collectedCurrency;
            }

            return allCollectedCurrency;
        }
        
        public static IdleObject GetPreviousBuiltObject(this IdleObjectsInteractor idleObjectsInteractor, IdleObject curIdleObject) {
            var idleObjects = idleObjectsInteractor.GetBuildedIdleObjects().OrderBy(idle => idle.info.number).ToList();
            
            if (idleObjects.Count == 1) return curIdleObject;
            
            var indexOfCurObject = idleObjects.IndexOf(curIdleObject);
            var indexOfPrevObject = indexOfCurObject - 1;
            var lastObjectsIndex = idleObjects.Count - 1;
            if (indexOfPrevObject < 0) indexOfPrevObject = lastObjectsIndex;

            return idleObjects[indexOfPrevObject];
        }

        public static IdleObject GetNextBuiltObject(this IdleObjectsInteractor idleObjectsInteractor, IdleObject curIdleObject) {
            var idleObjects = idleObjectsInteractor.GetBuildedIdleObjects().OrderBy(idle => idle.info.number).ToList();
            
            if (idleObjects.Count == 1) return curIdleObject;

            var indexOfCurObject = idleObjects.IndexOf(curIdleObject);
            var indexOfNextObject = indexOfCurObject + 1;
            if (indexOfNextObject == idleObjects.Count) indexOfNextObject = 0;

            return idleObjects[indexOfNextObject];
        }
    }
}