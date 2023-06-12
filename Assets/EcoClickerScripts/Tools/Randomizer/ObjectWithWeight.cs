namespace Ecorobotics {
	public class ObjectWithWeight {
		public int weight { get; set; }
		public object targetObject { get; set; }

		public ObjectWithWeight(object targetObject, int weight) {
			this.targetObject = targetObject;
			this.weight = weight;
		}

		public T GetObject<T>() {
			return (T) this.targetObject;
		}
	}
}