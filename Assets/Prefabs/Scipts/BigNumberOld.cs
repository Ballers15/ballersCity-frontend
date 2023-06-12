//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Numerics;
//using System.Security.Cryptography;
//using VavilichevGD.Translator;
//
//public enum NumberType{
//	Singles = 0,
//	Thousands = 1,
//	Millions = 2,
//	Billions = 3,
//	Trillions = 4,
//	AA = 5, BB = 6, CC = 7, DD = 8, EE = 9, FF = 10, GG = 11, HH = 12,
//	II = 13, JJ = 14, KK = 15, LL = 16, MM = 17, NN = 18, OO = 19, PP = 20,
//	QQ = 21, RR = 22, SS = 23, TT = 24, UU = 25, VV = 26, WW = 27, XX = 28,
//	YY = 29, ZZ = 30
//}
//
//[Serializable]
//public class BigNumberOld {
//
//	private static readonly string[] numbNames = {
//		"", "ID_THOUSAND", "ID_MILLION", "ID_BILLION", "ID_TRILLION", "ID_AA", "ID_BB",
//		"ID_CC", "ID_DD", "ID_EE", "ID_FF", "ID_GG", "ID_HH", "ID_II", "ID_JJ", "ID_KK", "ID_LL", "ID_MM", "ID_NN",
//		"ID_OO", "ID_PP", "ID_QQ", "ID_RR", "ID_SS", "ID_TT", "ID_UU", "ID_VV", "ID_WW", "ID_XX", "ID_YY", "ID_ZZ",
//	};
//
//	public BigInteger bigIntegerValue { get; set; }
//
//	/// <summary>
//	/// DO NOT USE IT. It is for property drawer;
//	/// </summary>
//	public NumberType editorNumberType;
//	/// <summary>
//	/// DO NOT USE IT. It is for property drawer;
//	/// </summary>
//	public int editorValue;
//
//	public BigNumberOld(string stringValue) {
//		bigIntegerValue = BigInteger.Parse(stringValue);
//	}
//
//	public BigNumberOld(NumberType type, float value) {
//		string stringValue = ConvertToStringValue(type, value);
//		bigIntegerValue = BigInteger.Parse(stringValue);
//	}
//
//	public BigNumberOld(BigInteger bigInteger) {
//		bigIntegerValue = bigInteger;
//	}
//
//	public BigNumberOld() {
//		bigIntegerValue = new BigInteger();
//	}
//
//	public BigNumberOld GetValueEditorSetupped() {
//		return new BigNumberOld(editorNumberType, editorValue);
//	}
//
//	public static BigNumberOld operator +(BigNumberOld num1, BigNumberOld num2) {
//		BigInteger bigSum = num1.bigIntegerValue + num2.bigIntegerValue;
//		return new BigNumberOld(bigSum);
//	}
//
//	public static BigNumberOld operator +(BigNumberOld num, int value) {
//		BigInteger bigSum = num.bigIntegerValue + value;
//		return new BigNumberOld(bigSum);
//	}
//
//	public static BigNumberOld operator -(BigNumberOld num1, BigNumberOld num2) {
//		BigInteger result = num1.bigIntegerValue - num2.bigIntegerValue;
//		if (result >= 0)
//			return new BigNumberOld(result);
//		return new BigNumberOld();
//	}
//
//	public static BigNumberOld operator * (BigNumberOld num, float mul) {
//		if (mul < 0)
//			throw new Exception(string.Format("Multiplicator cannot be negative: {0}", mul));
//
//		if (num.bigIntegerValue < 100) {
//			int intValue = (int)num.bigIntegerValue;
//			int result = Mathf.CeilToInt((intValue * mul));
//			BigInteger bitIntResult = new BigInteger(result);
//			return new BigNumberOld(bitIntResult);
//		}
//		else {
//			float roundedMul = (float)Math.Round(mul, 2);
//			int mul100 = Mathf.RoundToInt(roundedMul * 100);
//			BigInteger bitIntResult = (num.bigIntegerValue * mul100) / 100;
//			return new BigNumberOld(bitIntResult);
//		}
//		
//	}
//
//	public static BigNumberOld operator / (BigNumberOld num, float div) {
//		int div100 = Mathf.RoundToInt((float)Math.Round(div, 2) * 100);
//		BigInteger num100 = num.bigIntegerValue * 100;
//		BigInteger result = num100 / div100;
//		return new BigNumberOld(result);
//	}
//
//	public static BigNumberOld operator / (BigNumberOld dividendNumb, BigNumberOld divider) {
//		BigInteger result = dividendNumb.bigIntegerValue / divider.bigIntegerValue;
//		return new BigNumberOld(result);
//	}
//
//	public static double Divide(BigNumberOld dividend, BigNumberOld divider) {
//		return Math.Exp(BigInteger.Log(dividend.bigIntegerValue) - BigInteger.Log(divider.bigIntegerValue));
//	}
//
//	public static bool operator <= (BigNumberOld num1, BigNumberOld num2) {
//		return num1.bigIntegerValue <= num2.bigIntegerValue; 
//	}
//
//	public static bool operator <=(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue <= intValue;
//	}
//
//	public static bool operator >= (BigNumberOld num1, BigNumberOld num2) {
//		return num1.bigIntegerValue >= num2.bigIntegerValue;
//	}
//
//	public static bool operator >=(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue >= intValue;
//	}
//
//	public static bool operator <(BigNumberOld num1, BigNumberOld num2) {
//		return num1.bigIntegerValue < num2.bigIntegerValue;
//	}
//
//	public static bool operator <(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue < intValue;
//	}
//
//	public static bool operator >(BigNumberOld num1, BigNumberOld num2) {
//		return num1.bigIntegerValue > num2.bigIntegerValue;
//	}
//
//	public static bool operator >(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue > intValue;
//	}
//
//	public static bool operator ==(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue == intValue;
//	}
//
//	public static bool operator !=(BigNumberOld num, int intValue) {
//		return num.bigIntegerValue != intValue;
//	}
//
//
//	public static BigNumberOld RandomRange(BigNumberOld num1, BigNumberOld num2) {
//		var random = RandomNumberGenerator.Create();
//		BigInteger randomInteger = RandomInRange(random, num1.bigIntegerValue, num2.bigIntegerValue);
//		return new BigNumberOld(randomInteger);
//	}
//
//	private static BigInteger RandomInRange(RandomNumberGenerator rng, BigInteger min, BigInteger max) {
//		if (min > max) {
//			var buff = min;
//			min = max;
//			max = buff;
//		}
//
//		// offset to set min = 0
//		BigInteger offset = -min;
//		min = 0;
//		max += offset;
//
//		var value = randomInRangeFromZeroToPositive(rng, max) - offset;
//		return value;
//	}
//
//	private static BigInteger randomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max) {
//		BigInteger value;
//		var bytes = max.ToByteArray();
//
//		// count how many bits of the most significant byte are 0
//		// NOTE: sign bit is always 0 because `max` must always be positive
//		byte zeroBitsMask = 0b00000000;
//
//		var mostSignificantByte = bytes[bytes.Length - 1];
//
//		// we try to set to 0 as many bits as there are in the most significant byte, starting from the left (most significant bits first)
//		// NOTE: `i` starts from 7 because the sign bit is always 0
//		for (var i = 7; i >= 0; i--) {
//			// we keep iterating until we find the most significant non-0 bit
//			if ((mostSignificantByte & (0b1 << i)) != 0) {
//				var zeroBits = 7 - i;
//				zeroBitsMask = (byte)(0b11111111 >> zeroBits);
//				break;
//			}
//		}
//
//		do {
//			rng.GetBytes(bytes);
//
//			// set most significant bits to 0 (because `value > max` if any of these bits is 1)
//			bytes[bytes.Length - 1] &= zeroBitsMask;
//
//			value = new BigInteger(bytes);
//
//			// `value > max` 50% of the times, in which case the fastest way to keep the distribution uniform is to try again
//		} while (value > max);
//
//		return value;
//	}
//
//
//	/// <summary>
//	/// You can convert value from 0 to 999.9 of type to string line. For example value 123.4 of type billion will be converted to 123400000000 string.
//	/// WARNING: Value must be from 0 to 999.9.
//	/// </summary>
//	/// <param name="type"></param>
//	/// <param name="value"></param>
//	public static string ConvertToStringValue(NumberType type, float value) {
//		if (value < 1000 && value >= 0) {
//			float clampedValue = Mathf.Clamp(value, 0f, 999.9f);
//			clampedValue = (float)Math.Round(clampedValue, 1);
//
//			string stringValue = "";
//			int blocksCount = (int)type + 1;
//			for (int i = blocksCount - 1; i >= 0; i--) {
//				if (clampedValue > 0) {
//					int blockValue = Mathf.FloorToInt(clampedValue);
//					stringValue += blockValue;
//					clampedValue = Mathf.RoundToInt((clampedValue - blockValue) * 1000);
//				}
//				else
//					stringValue += "000";
//			}
//
//			while (stringValue.Length > 0 && stringValue[0] == '0')
//				stringValue = stringValue.Remove(0, 1);
//
//			if (string.IsNullOrEmpty(stringValue))
//				stringValue = "0";
//			return stringValue;
//		}
//		throw new Exception("The value must be from 0 to 1000, you want to convert: " + value);
//	}
//
//	public override string ToString() {
//		string stringValue = bigIntegerValue.ToString();
//		string[] blocks = SplitToArrayOf3(stringValue);
//		int lastIndex = blocks.Length - 1;
//		if (lastIndex >= 0) {
//			if (lastIndex >= numbNames.Length)
//				throw new Exception("Index out of range. The lenth of value is too long. Length: " + (lastIndex + 1).ToString());
//
//			string finalValue = blocks[lastIndex];
//			if (lastIndex > 0) {
//				string valueAfterComa = blocks[lastIndex - 1][0].ToString();
//				if (valueAfterComa != "0")
//					finalValue += "." + valueAfterComa;
//			}
//			string currencyName = numbNames[lastIndex];
//			string translatedName = currencyName;
//			if (!string.IsNullOrEmpty(translatedName))
//				translatedName = Translator.instance.GetTranslate(translatedName);
//			return finalValue + translatedName;
//		}
//		return "0";
//	}
//
//	public string ToStringFull() {
//		return bigIntegerValue.ToString();
//	} 
//
//	private string[] SplitToArrayOf3(string text) {
//		List<string> splittedList = new List<string>();
//
//		for (int i = text.Length - 1; i >= 0; i -= 3) {
//			string block = "";
//			block = text[i] + block;
//			if (i >= 1)
//				block = text[i - 1] + block;
//			if (i >= 2)
//				block = text[i - 2] + block;
//			splittedList.Add(block);
//		}
//		return splittedList.ToArray();
//	}
//
//	public void Clean() {
//		bigIntegerValue = 0;
//	}
//
//}