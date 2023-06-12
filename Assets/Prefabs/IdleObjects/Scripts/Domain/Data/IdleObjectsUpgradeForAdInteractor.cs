using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Domain.Utils;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

public class IdleObjectsUpgradeForAdInteractor : Interactor
{
    #region CONST
    
    private string CONFIG_PATH = "IdleObjectUpgradeForADConfig";
    private const int MAX_LEVEL = 1000;

    #endregion
    public override bool onCreateInstantly { get; } = true;
    private List<IdleObject> buildedIdleObjects;
    private List<IdleObject> canUpgradeForAdIdlesMap;
    private IdleObjectsInteractor idleObjectsInteractor;
    private IdleObject selectedForAdUpgrade;
    private IdleObjectUpgradeForADConfig config;

    protected override void Initialize()
    {
        base.Initialize();
        this.idleObjectsInteractor = this.GetInteractor<IdleObjectsInteractor>();
        this.config = Resources.Load<IdleObjectUpgradeForADConfig>(CONFIG_PATH);
    }

    public void RefreshData()
    {
        this.canUpgradeForAdIdlesMap = new List<IdleObject>();
        this.selectedForAdUpgrade = null;
        this.buildedIdleObjects = this.idleObjectsInteractor.GetBuildedIdleObjects().ToList();

        if (this.buildedIdleObjects.Count < config.minIdleObjectNumber) return;

        this.buildedIdleObjects.Sort((idle1, idle2) => idle2.info.number.CompareTo(idle1.info.number));

        foreach (var idle in this.buildedIdleObjects)
        {
            this.canUpgradeForAdIdlesMap.Add(idle);
        }
        
        /*for (var i = 0; i < this.config.lastBuildedObjectsCount; i++)
        {
            var idle = this.buildedIdleObjects[i];

            if (idle.info.number < this.config.minIdleObjectNumber) continue;

            this.canUpgradeForAdIdlesMap.Add(idle);
        }*/
    }

    public bool CanUpgradeForAd(IdleObject idleObject)
    {
        var searchedIdle = this.canUpgradeForAdIdlesMap.Find(idle => idle.info.id == idleObject.info.id);
        //return (searchedIdle != null && this.selectedForAdUpgrade == null);
        return (searchedIdle != null);
    }

    public int GetUpgradeLevelsCount(IdleObject idleObject)
    {
        var fullObjectsIncome = this.idleObjectsInteractor.GetFullIncomeByIdleObjectsForTime(this.config.incomePeriodSeconds);
        var priceImprovementSum = BigNumber.zero;
        var curIdleLevel = idleObject.state.level;
        var upgradeDiscount = 1f - this.config.upgradeDiscount;
        
        while (priceImprovementSum < fullObjectsIncome)
        {
            if (idleObject.state.level == curIdleLevel)
            {
                priceImprovementSum += idleObject.state.priceImprovement * upgradeDiscount;
            }
            else
            {
                var priceImprovement = idleObject.info.priceImproveDefault * 
                                       Math.Pow(idleObject.info.priceStep, curIdleLevel) * upgradeDiscount;
                priceImprovementSum += priceImprovement;
            }

            if (curIdleLevel < MAX_LEVEL)
            {
                curIdleLevel++;    
            }
            else
            {
                break;
            }
        }

        var resultLevelsCount = curIdleLevel - idleObject.state.level;
        return resultLevelsCount;
    }

    public void SelectAsUpgradeForAd(IdleObject idle)
    {
        this.selectedForAdUpgrade = idle;
    }
}
