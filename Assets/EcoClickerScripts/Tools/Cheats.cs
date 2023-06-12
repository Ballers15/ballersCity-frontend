using SinSity.Core;
using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace EcoClickerScripts.Tools {
	public class Cheats {

		public void AddCleanEnergy(BigNumber value) {
			Bank.AddSoftCurrency(value, this);
			Bank.uiBank.AddSoftCurrency(this, value);
		}

		public void AddGems(int value) {
			Bank.AddHardCurrency(value, this);
			Bank.uiBank.AddHardCurrency(this, value);
		}

		public void AddExperience() {
			var idleObject = Game.GetInteractor<IdleObjectsInteractor>().GetIdleObject("io_1");
			Game.GetInteractor<IdleObjectExperienceInteractor>()
				.OnIdleObjectBuilt(idleObject, idleObject.state);
		}
		
	}
}