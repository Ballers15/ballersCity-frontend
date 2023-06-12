using System;
using UnityEngine;
using System.Numerics;
using System.Security.Cryptography;

namespace VavilichevGD.Tools {
	[Serializable]
	public struct BigNumber : ISerializationCallbackReceiver {
		
		#region CONSTANTS

		public const string FORMAT_FULL = "FULL";
		public const string FORMAT_XXX_C = "XXX C";
		public const string FORMAT_XXXC = "XXXC";
		public const string FORMAT_XXX_XX_C = "XXX.XX C";
		public const string FORMAT_XXX_XXC = "XXX.XXC";
		public const string FORMAT_XXX_X_C = "XXX.X C";
		public const string FORMAT_XXX_XC = "XXX.XC";
		public const string FORMAT_DYNAMIC_3_C = "DYNAMIC3 C";
		public const string FORMAT_DYNAMIC_3C = "DYNAMIC3C";
		public const string FORMAT_DYNAMIC_4_C = "DYNAMIC4 C";
		public const string FORMAT_DYNAMIC_4C = "DYNAMIC4C";
		
		#endregion
		
		
		[SerializeField] private float cutValue;
		[SerializeField] private BigNumberOrder order;

		
		internal BigInteger bigIntegerValue { get; private set; }
		private bool isInitialized { get; set; }

		
		
		public static BigNumber maxValue => GetMaxValue();
		public static BigNumber zero => new BigNumber(0);

		private static BigNumber GetMaxValue() {
			int countOfOrders = Enum.GetNames(typeof(BigNumberOrder)).Length;
			string finalValueString = "";
			for (int i = 0; i < countOfOrders; i++)
				finalValueString = $"{finalValueString}999";

			return new BigNumber(finalValueString);
		}
		

		
		#region CONSTRUCTORS
		
		public BigNumber(BigNumber bigNumber)
		{
			this.bigIntegerValue = bigNumber.bigIntegerValue;
			this.order = bigNumber.GetOrder();
			this.cutValue = bigNumber.GetCutValue();
			this.isInitialized = true;
		}

		public BigNumber(BigInteger bigInteger) {
			this.bigIntegerValue = bigInteger;
			var max = maxValue;
			if (bigInteger > max.bigIntegerValue)
				bigIntegerValue = max.bigIntegerValue;

			this.order = GetOrder(this.bigIntegerValue);
			this.cutValue = GetCutValue(this.bigIntegerValue);
			this.isInitialized = true;
		}

		public BigNumber(int value) {
			this.bigIntegerValue = value;
			this.order = GetOrder(this.bigIntegerValue);
			this.cutValue = GetCutValue(this.bigIntegerValue);
            
			this.isInitialized = true;
		}

		public BigNumber(string strValue) {
			this.bigIntegerValue = BigInteger.Parse(strValue);
			this.order = GetOrder(this.bigIntegerValue);
			this.cutValue = GetCutValue(this.bigIntegerValue);
            
			this.isInitialized = true;
		}

		public BigNumber(BigNumberOrder order, float cutFloat) {
			var intValue = Mathf.FloorToInt(cutFloat);
			var decValue = Mathf.RoundToInt((cutFloat - intValue) * 1000);
			var addZeroBlockCount = decValue == 0 ? (int) order : (int) order - 1;
			var strValue = intValue.ToString();

			if (decValue > 0)
				strValue = $"{strValue}{decValue}";

			if ((int) order >= 1) {
				for (int i = 0; i < addZeroBlockCount; i++)
					strValue = $"{strValue}000";
			}

			this.order = order;
			this.cutValue = cutFloat;
			this.bigIntegerValue = BigInteger.Parse(strValue);
			this.isInitialized = true;
		}

		#endregion


		#region GET

		public float GetCutValue() {
			return this.cutValue;
		}

		private static float GetCutValue(BigInteger bigInteger) {
			var fullString = bigInteger.ToString();
			var length = fullString.Length;
			if (length < 4)
				return Convert.ToInt32(fullString);

			var simbolsCount = (length - 1) % 3 + 2;
			var simbols = fullString.Substring(0, simbolsCount);
			var intValue = Convert.ToInt32(simbols);
			return intValue / 10f;
		}
		
		
		public BigNumberOrder GetOrder() {
			return this.order;
		}
        
		private static BigNumberOrder GetOrder(BigInteger bigInteger) {
			var fullString = bigInteger.ToString();
			var length = fullString.Length;
			return (BigNumberOrder) ((length - 1) / 3);
		}

		#endregion
	

		#region Calculations

		
		#region BigNumber && BigNumber


		public static BigNumber operator +(BigNumber num1, BigNumber num2) {
			BigInteger bigSum = num1.bigIntegerValue + num2.bigIntegerValue;
			return Clamp(new BigNumber(bigSum));
		}

		public static BigNumber operator -(BigNumber num1, BigNumber num2) {
			BigInteger result = num1.bigIntegerValue - num2.bigIntegerValue;
			return Clamp(new BigNumber(result));
		}

		public static BigNumber operator /(BigNumber dividedNumb, BigNumber divider) {
			BigInteger result = dividedNumb.bigIntegerValue / divider.bigIntegerValue;
			return Clamp(new BigNumber(result));
		}

		public static BigNumber operator *(BigNumber num1, BigNumber num2) {
			BigInteger result = num1.bigIntegerValue * num2.bigIntegerValue;
			return Clamp(new BigNumber(result));
		}

		public static BigNumber operator %(BigNumber dividedNumb, int value)
		{
			BigInteger result = dividedNumb.bigIntegerValue % value;
			return Clamp(new BigNumber(result));
		}
		#endregion


		#region BigNumber && integer


		public static BigNumber operator +(BigNumber num, int value) {
			BigInteger result = num.bigIntegerValue + value;
			return Clamp(new BigNumber(result));
		}
		
		public static BigNumber operator -(BigNumber num, int value) {
			BigInteger result = num.bigIntegerValue - value;
			return Clamp(new BigNumber(result));
		}

		public static BigNumber operator *(BigNumber num1, int value) {
			BigInteger result = num1.bigIntegerValue * value;
			return Clamp(new BigNumber(result));
		}
		
		public static BigNumber operator /(BigNumber dividedNumb, int value) {
			BigInteger result = dividedNumb.bigIntegerValue / value;
			return Clamp(new BigNumber(result));
		}
		
		
		#endregion
		

		#region BigNumber && float


		public static BigNumber operator *(BigNumber num, float mul) {
			if (mul < 0)
				throw new Exception(string.Format("Multiplicator cannot be negative: {0}", mul));
			
			if (num.bigIntegerValue < 100) {
				int intValue = (int) num.bigIntegerValue;
				int result = Mathf.CeilToInt((intValue * mul));
				BigInteger bigIntResult = new BigInteger(result);
				return Clamp(new BigNumber(bigIntResult));
			}

			float roundedMul = (float) Math.Round(mul, 2);
			int mul100 = Mathf.RoundToInt(roundedMul * 100);
			BigInteger bitIntResult = (num.bigIntegerValue * mul100) / 100;
			return Clamp(new BigNumber(bitIntResult));
		}

		public static BigNumber operator /(BigNumber num, float div) {
			int div100 = Mathf.RoundToInt((float) Math.Round(div, 2) * 100);
			BigInteger num100 = num.bigIntegerValue * 100;
			BigInteger result = num100 / div100;
			return Clamp(new BigNumber(result));
		}
		

		#endregion

		
		#region BigNumber && double

		public static BigNumber operator +(BigNumber num, double value) {
			BigInteger result = num.bigIntegerValue + new BigInteger(value);
			return Clamp(new BigNumber(result));
		}
		
		public static BigNumber operator -(BigNumber num, double value) {
			BigInteger result = num.bigIntegerValue - new BigInteger(value);
			return Clamp(new BigNumber(result));
		}

		public static BigNumber operator *(BigNumber num1, double value) {
			BigInteger result = num1.bigIntegerValue * new BigInteger(value);
			return Clamp(new BigNumber(result));
		}
		
		public static BigNumber operator /(BigNumber dividedNumb, double value) {
			BigInteger result = dividedNumb.bigIntegerValue / new BigInteger(value);
			return Clamp(new BigNumber(result));
		}

		#endregion
		

		private static BigNumber Clamp(BigNumber clampingValue) {
			if (clampingValue.bigIntegerValue < 0)
				return new BigNumber(0);


			var countOfOrders = Enum.GetNames(typeof(BigNumberOrder)).Length;
			var maxValueLength = countOfOrders * 3;		// Every order contains 3 digits.
			var clampingValueLength = clampingValue.ToString(FORMAT_FULL).Length;

			if (clampingValueLength > maxValueLength)
				return maxValue;

			return clampingValue;
		}

		public static BigNumber Clamp(BigNumber clampingValue, BigNumber minValue, BigNumber _maxValue) {
			BigNumber min = Max(minValue, BigNumber.zero);
			if (clampingValue < min)
				return min;

			BigNumber max = Min(_maxValue, BigNumber.maxValue);
			if (clampingValue > max)
				return max;

			return clampingValue;
		}

		public static BigNumber Min(params BigNumber[] numbers) {
			BigNumber min = BigNumber.maxValue;
			foreach (BigNumber number in numbers) {
				if (number < min)
					min = number;
			}

			return min;
		}

		public static BigNumber Max(params BigNumber[] numbers) {
			BigNumber max = BigNumber.zero;
			foreach (BigNumber number in numbers) {
				if (number > max)
					max = number;
			}

			return max;
		}

		public static double DivideToDouble(BigNumber dividedNumb, BigNumber divider) {
			return Math.Exp(BigInteger.Log(dividedNumb.bigIntegerValue) - BigInteger.Log(divider.bigIntegerValue));
		}

		public static double GetLog(BigNumber value, int logBase)
		{
			return BigInteger.Log(value.bigIntegerValue, logBase);
		}

		#endregion


		#region Compairs


		public static bool operator <=(BigNumber num1, BigNumber num2) {
			return num1.bigIntegerValue <= num2.bigIntegerValue;
		}

		public static bool operator >=(BigNumber num1, BigNumber num2) {
			return num1.bigIntegerValue >= num2.bigIntegerValue;
		}



		public static bool operator <(BigNumber num1, BigNumber num2) {
			return num1.bigIntegerValue < num2.bigIntegerValue;
		}

		public static bool operator >(BigNumber num1, BigNumber num2) {
			return num1.bigIntegerValue > num2.bigIntegerValue;
		}



		public static bool operator >=(BigNumber num, int intValue) {
			return num.bigIntegerValue >= intValue;
		}

		public static bool operator <=(BigNumber num, int intValue) {
			return num.bigIntegerValue >= intValue;
		}



		public static bool operator <(BigNumber num, int intValue) {
			return num.bigIntegerValue < intValue;
		}

		public static bool operator >(BigNumber num, int intValue) {
			return num.bigIntegerValue > intValue;
		}



		public static bool operator ==(BigNumber num, int intValue) {
			return num.bigIntegerValue == intValue;
		}

		public static bool operator !=(BigNumber num, int intValue) {
			return num.bigIntegerValue != intValue;
		}


		#endregion


		#region RandomRange

		public static BigNumber RandomRange(BigNumber num1, BigNumber num2) {
			var random = RandomNumberGenerator.Create();
			BigInteger randomInteger = RandomInRange(random, num1.bigIntegerValue, num2.bigIntegerValue);
			return new BigNumber(randomInteger);
		}

		private static BigInteger RandomInRange(RandomNumberGenerator rng, BigInteger min, BigInteger max) {
			if (min > max) {
				var buff = min;
				min = max;
				max = buff;
			}

			// offset to set min = 0
			BigInteger offset = -min;
			max += offset;

			var value = RandomInRangeFromZeroToPositive(rng, max) - offset;
			return value;
		}

		private static BigInteger RandomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max) {
			BigInteger value;
			var bytes = max.ToByteArray();

			// count how many bits of the most significant byte are 0
			// NOTE: sign bit is always 0 because `max` must always be positive
			byte zeroBitsMask = 0b00000000;

			var mostSignificantByte = bytes[bytes.Length - 1];

			// we try to set to 0 as many bits as there are in the most significant byte, starting from the left (most significant bits first)
			// NOTE: `i` starts from 7 because the sign bit is always 0
			for (var i = 7; i >= 0; i--) {
				// we keep iterating until we find the most significant non-0 bit
				if ((mostSignificantByte & (0b1 << i)) != 0) {
					var zeroBits = 7 - i;
					zeroBitsMask = (byte) (0b11111111 >> zeroBits);
					break;
				}
			}

			do {
				rng.GetBytes(bytes);

				// set most significant bits to 0 (because `value > max` if any of these bits is 1)
				bytes[bytes.Length - 1] &= zeroBitsMask;

				value = new BigInteger(bytes);

				// `value > max` 50% of the times, in which case the fastest way to keep the distribution uniform is to try again
			} while (value > max);

			return value;
		}


		#endregion


		#region ToString

		public override string ToString() {
			return this.ToString(FORMAT_XXX_XC);
		}

		public string ToString(string format) {
			return this.Format(format);
		}
		
		public string ToString(string format, IBigNumberDictionary dictionary) {
			return this.Format(format, dictionary);
		}

		private string Format(string format, IBigNumberDictionary dictionary = null) {
			
			format = format.ToUpperInvariant();

			if (String.IsNullOrEmpty(format) || (this.bigIntegerValue < 1000 && format != FORMAT_FULL))
				format = FORMAT_XXX_C;
				
			var fullNumberToString = this.bigIntegerValue.ToString();
			var numberLength = fullNumberToString.Length;

			var olderNumbersLength = numberLength % 3 == 0 ? 3 : numberLength % 3;
			var olderNumberString = fullNumberToString.Substring(0, olderNumbersLength);
			var orderToString = dictionary != null ? dictionary.GetTranslatedOrder(order) : order.ToString();
			if (order == 0)
				orderToString = "";
			
			var finalStringWithoutOrder = "";

			switch (format)
			{
				case FORMAT_XXX_XX_C:
					finalStringWithoutOrder =
						this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, true);
					break;

				case FORMAT_XXX_XXC:
					finalStringWithoutOrder =
						this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, false);
					break;
				
				case FORMAT_XXX_X_C:
					finalStringWithoutOrder =
						this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, true);
					break;
				
				case FORMAT_XXX_XC:
					finalStringWithoutOrder =
						this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, false);
					break;
				
				case FORMAT_XXX_C:
					finalStringWithoutOrder = $"{olderNumberString} ";
					break;
				
				case FORMAT_XXXC:
					finalStringWithoutOrder = $"{olderNumberString}";
					break;
				
				case FORMAT_FULL:
					return this.bigIntegerValue.ToString();

				case FORMAT_DYNAMIC_3_C:
					switch (olderNumbersLength) {
						case 1:
							finalStringWithoutOrder =
								this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, true);
							break;
						case 2:
							finalStringWithoutOrder =
								this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, true);
							break;
						case 3:
							finalStringWithoutOrder = $"{olderNumberString} ";
							break;
					}
					break;
				
				case FORMAT_DYNAMIC_3C:
					switch (olderNumbersLength) {
						case 1:
							finalStringWithoutOrder =
								this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, false);
							break;
						case 2:
							finalStringWithoutOrder =
								this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, false);
							break;
						case 3:
							finalStringWithoutOrder = $"{olderNumberString}";
							break;
					}
					break;
				
				case FORMAT_DYNAMIC_4_C:
					if (olderNumbersLength < 3) {
						finalStringWithoutOrder =
							this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, true);
					}
					else {
						finalStringWithoutOrder =
							this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, true);
					}
					break;
				
				case FORMAT_DYNAMIC_4C:
					if (olderNumbersLength < 3) {
						finalStringWithoutOrder =
							this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 2, false);
					}
					else {
						finalStringWithoutOrder =
							this.GetFinalStringWithoutOrder(fullNumberToString, olderNumberString, 1, false);
					}
					break;
				
				default:
					throw new FormatException(String.Format("The '{0}' format string is not supported.", format));
			}
			
			return $"{finalStringWithoutOrder}{orderToString}";
		}

		private string GetFinalStringWithoutOrder(string fullNumberToString, string olderNumberString, int youngerNumbersLength, bool withSpace) {
			int olderNumbersLength = olderNumberString.Length;
			var youngerNumberString = $"{fullNumberToString.Substring(olderNumbersLength, youngerNumbersLength)}";
			if (Convert.ToInt32(youngerNumberString) == 0)
				youngerNumberString = "";
			else
				youngerNumberString = $".{youngerNumberString}";
			return $"{olderNumberString}{youngerNumberString}" + (withSpace ? " " : "");
		}

		#endregion
		
		
		
		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize() {
			int intValue = Mathf.FloorToInt(cutValue);
			int decValue = Mathf.RoundToInt((cutValue - intValue) * 1000);

			int addZeroBlockCount = decValue == 0 ? (int) order : (int) order - 1;
			string strValue = intValue.ToString();
			if (decValue > 0)
				strValue = $"{strValue}{decValue}";

			if ((int) order >= (int) BigNumberOrder.Thousands) {
				for (int i = 0; i < addZeroBlockCount; i++)
					strValue = $"{strValue}000";
			}

			bigIntegerValue = BigInteger.Parse(strValue);
			isInitialized = true;
		}
		
		
		
		public bool Equals(BigNumber other) {
			return cutValue.Equals(other.cutValue) && order == other.order && bigIntegerValue.Equals(other.bigIntegerValue) && isInitialized == other.isInitialized;
		}

		public override bool Equals(object obj) {
			return obj is BigNumber other && Equals(other);
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = cutValue.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) order;
				hashCode = (hashCode * 397) ^ bigIntegerValue.GetHashCode();
				hashCode = (hashCode * 397) ^ isInitialized.GetHashCode();
				return hashCode;
			}
		}
		
	}
}