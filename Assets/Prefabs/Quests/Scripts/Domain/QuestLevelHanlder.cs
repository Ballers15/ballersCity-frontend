using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "QuestLevelHanlder",
        menuName = "Domain/Quests/New QuestLevelHanlder"
    )]
    public sealed class QuestLevelHanlder : ScriptableProfileLevelHandler
    {
        public override void OnProfileLevelRisen()
        {
            var mainQuestInteractor = Game.GetInteractor<MainQuestsInteractor>();
            var miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
            var isMainQuestsUnlocked = mainQuestInteractor.isQuestsUnlocked;
            var isMinoQuestsUnlocked = miniQuestInteractor.isQuestsUnlocked;
            if (isMainQuestsUnlocked && isMinoQuestsUnlocked)
            {
                return;
            }

            if (isMainQuestsUnlocked ^ isMinoQuestsUnlocked)
            {
                throw new Exception("Requeired that both quests is unlocked!");
            }

#if DEBUG
            Debug.Log("<color=green>UNLOCK QUESTS</color>");
#endif
            mainQuestInteractor.UnlockQuests(this);
            miniQuestInteractor.UnlockQuests(this);
            this.ReceiveReward();
        }
    }
}