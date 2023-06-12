using Orego.Util;
using SinSity.Domain;

namespace SinSity.UI
{
    public interface IUIPanelResearch
    {
        #region Event

        AutoEvent<IUIPanelResearch> OnStartBtnClickEvent { get; }

        AutoEvent<IUIPanelResearch> OnGetRewardBtnClickEvent { get; }

        #endregion

        string researchId { get; }

        ResearchObject researchObjectCurrent { get; }

        void Setup(ResearchObject researchObject);

        void SetStateCanStart();

        void SetStateTimerWork();

        void UpdateTimerValue();

        void SetStateAwaitingReward();
    }
}