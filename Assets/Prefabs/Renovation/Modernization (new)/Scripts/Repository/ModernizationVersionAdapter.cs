using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public class ModernizationVersionAdapter {

        private RenovationRepository renovationRepository;
        private ModernizationRepository modernizationRepository;

        public ModernizationVersionAdapter(
            RenovationRepository renovationRepository,
            ModernizationRepository modernizationRepository) {

            this.renovationRepository = renovationRepository;
            this.modernizationRepository = modernizationRepository;

        }

        public void Adapt() {
            if (renovationRepository == null)
                return;

            var renovationStatistics = renovationRepository.Get();
            if (renovationStatistics.level == 1 && renovationStatistics.passedQuestCount == 0)
                return;
            
            if (this.modernizationRepository.data.multiplier == 1)
            {
                var renovationLevel = renovationStatistics.level;
                var modernizationMultiplier = renovationLevel;
                var isAvailable = true;
                this.modernizationRepository.SetDataValues(modernizationMultiplier, this.modernizationRepository.data.scores,isAvailable);
                Logging.Log($"MODERNIZATION ADAPTER: Old multiplier: x{Mathf.Pow(2, renovationLevel - 1)}, and new: {Mathf.RoundToInt(modernizationMultiplier * 100)}%");
            }
            
        }
        
    }
}