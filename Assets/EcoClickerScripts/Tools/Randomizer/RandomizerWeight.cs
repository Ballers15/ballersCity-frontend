
using System;
using Random = UnityEngine.Random;

namespace Ecorobotics {
	public sealed class RandomizerWeight : IRandomizer {
		
		public ObjectWithWeight[] objects { get; }
		public int totalWeight { get; }
		

		public RandomizerWeight(ObjectWithWeight[] objects) {
			this.objects = objects;

			this.totalWeight = 0;
			foreach (var obj in this.objects)
				this.totalWeight += obj.weight;
		}

		public T GetRandom<T>() where T : class {
			var rWeight = Random.Range(0, this.totalWeight);
			var weightSum = 0;

			foreach (var obj in this.objects) {
				weightSum += obj.weight;
				if (rWeight <= weightSum)
					return obj.targetObject as T;
			}

			return null;
		}
		
		public T GetRandomBaseType<T>(){
			var rWeight = Random.Range(0, this.totalWeight);
			var weightSum = 0;

			foreach (var obj in this.objects) {
				weightSum += obj.weight;
				if (rWeight <= weightSum)
					return (T) obj.targetObject;
			}

			return default(T);
		}
	}
}