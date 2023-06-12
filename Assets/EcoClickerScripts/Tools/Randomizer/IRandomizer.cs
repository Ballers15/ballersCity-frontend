namespace Ecorobotics {
	public interface IRandomizer {
		T GetRandom<T>() where T : class;
	}
}