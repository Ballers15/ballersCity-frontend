using System;
using System.Collections;
using System.Collections.Generic;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupDailyRewards : UIPopupAnim<UIPopupDailyRewardsProperties, UIPopupArgs>
    {
        private DailyRewardInteractor dailyRewardInteractor;

        protected override void Awake()
        {
            base.Awake();
            this.dailyRewardInteractor = this.GetInteractor<DailyRewardInteractor>();
            this.dailyRewardInteractor.OnDailyBonusReceivedEvent += this.OnDailyBonusReceived;
        }

        private void OnEnable()
        {
            this.properties.buttonReceiveRewards.AddListener(this.OnReceiveRewardClick);
        }

        protected override void Start()
        {
            base.Start();
            this.Refresh();
            this.StartCoroutine(this.WaitSecond());
        }

        private IEnumerator WaitSecond()
        {
            yield return new WaitForSeconds(0.2f);
            SFX.PlaySFX(this.properties.audioClipShow);
        }

        private void OnDisable()
        {
            this.properties.buttonReceiveRewards.RemoveListeners();
        }

        public void Refresh()
        {
            var rewardArgsRange = this.GetRewardArgsRange();
            var uiRewardCards = this.properties.rewardCards;
            for (var i = 0; i < uiRewardCards.Length; i++)
            {
                var uiRewardCard = uiRewardCards[i];
                var rewardArgs = rewardArgsRange[i];
                uiRewardCard.Setup(rewardArgs);
            }
        }

        private List<UIWidgetRewardCard.Args> GetRewardArgsRange()
        {
            var resultList = new List<UIWidgetRewardCard.Args>();
            var rewardsPipeline = this.dailyRewardInteractor.dailyRewardsPipeline;
            var dayCount = rewardsPipeline.dayCount;
            var currentDay = this.dailyRewardInteractor.day;
            var currentDayIndex = currentDay - 1;

            var currentRewardArgs = new UIWidgetRewardCard.Args
            {
                isToday = true,
                isReceived = false,
                dayNumber = currentDay,
                rewardInfo = rewardsPipeline.GetReward(currentDayIndex)
            };

            //First day:
            if (currentDay == 1)
            {
                resultList.Add(currentRewardArgs);
                for (var i = 1; i < 5; i++)
                {
                    var args = new UIWidgetRewardCard.Args
                    {
                        isToday = false,
                        isReceived = false,
                        dayNumber = i + currentDay,
                        rewardInfo = rewardsPipeline.GetReward(i)
                    };
                    resultList.Add(args);
                }

                return resultList;
            }

            //Last days:
            if (currentDay >= dayCount - 3)
            {
                var startIndex = dayCount - 5;
                var lastIndex = currentDay - 2;
                for (var i = startIndex; i <= lastIndex; i++)
                {
                    var args = new UIWidgetRewardCard.Args
                    {
                        isToday = false,
                        isReceived = true,
                        dayNumber = i + 1,
                        rewardInfo = rewardsPipeline.GetReward(i)
                    };
                    resultList.Add(args);
                }

                resultList.Add(currentRewardArgs);
                for (var i = currentDay; i < dayCount; i++)
                {
                    var args = new UIWidgetRewardCard.Args
                    {
                        isToday = false,
                        isReceived = false,
                        dayNumber = i + 1,
                        rewardInfo = rewardsPipeline.GetReward(i)
                    };
                    resultList.Add(args);
                }

                return resultList;
            }

            //Middle:
            for (var i = -1; i < 4; i++)
            {
                var args = new UIWidgetRewardCard.Args
                {
                    isToday = false,
                    isReceived = false,
                    dayNumber = i + currentDay,
                    rewardInfo = rewardsPipeline.GetReward(currentDayIndex + i)
                };
                resultList.Add(args);
            }

            resultList[0].isReceived = true;
            resultList[1].isToday = true;
            return resultList;
        }

        #region ClickEvents

        private void OnReceiveRewardClick()
        {
            this.dailyRewardInteractor.ReceiveReward();
            this.Hide();
            SFX.PlaySFX(this.properties.audioClipGetClick);
        }

        #endregion

        #region DailyRewardEvents

        private void OnDailyBonusReceived(Reward reward)
        {
        }

        #endregion
    }
}