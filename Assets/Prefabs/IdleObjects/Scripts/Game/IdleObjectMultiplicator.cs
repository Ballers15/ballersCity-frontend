using System.Collections.Generic;
//using Facebook.Unity;

namespace SinSity.Core
{
    public sealed class IdleObjectMultiplicator<T> where T : Coefficient
    {
        private readonly Dictionary<string, T> coefficientMap;

        public IdleObjectMultiplicator()
        {
            this.coefficientMap = new Dictionary<string, T>();
        }

        public void SetCoefficient(string id, T coefficient)
        {
            this.coefficientMap[id] = coefficient;
        }

        public void SetCoefficients(Dictionary<string, T> coefficients)
        {
            foreach (var coefficient in coefficients)
            {
                this.coefficientMap[coefficient.Key] = coefficient.Value;
            }
        }

        public void RemoveCoefficient(string id)
        {
            this.coefficientMap.Remove(id);
        }

        public List<T> GetMultipliers()
        {
            var list = new List<T>();
            var coefficients = this.coefficientMap.Values;
            foreach (var coefficient in coefficients)
            {
                var clone = (T) coefficient.Clone();
                list.Add(clone);
            }
            return list;
        }

        public double GetTotalMultiplier()
        {
            double totalMultiplier = 1;
            var coefficients = this.coefficientMap.Values;
            foreach (var coefficient in coefficients)
            {
                totalMultiplier *= coefficient.value;
            }

            return totalMultiplier;
        }
    }
}