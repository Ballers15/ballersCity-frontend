using System;
using UnityEngine;

namespace VavilichevGD {
	public static class Loader {

		public static void SetBool(string prefKey, bool value) {
			PlayerPrefs.SetInt(prefKey, BoolToInteger(value));
		}

		private static int BoolToInteger(bool value) {
			if (value)
				return 1;
			return 0;
		}

		public static bool LoadBool(string prefKey, bool defaultValue = true) {
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetInt(prefKey, BoolToInteger(defaultValue));
			int intValue = PlayerPrefs.GetInt(prefKey);
			return IntToBool(intValue);
		}

		private static bool IntToBool(int value) {
			return value != 0;
		}



		public static void SetFloat(string prefKey, float value) {
			PlayerPrefs.SetFloat(prefKey, value);
		}

		public static float LoadFloat(string prefKey, float defaultValue = 1f) {
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetFloat(prefKey, defaultValue);
			return PlayerPrefs.GetFloat(prefKey);
		}


		public static void SetInt(string prefKey, int value) {
			PlayerPrefs.SetInt(prefKey, value);
		}

		public static int LoadInteger(string prefKey, int defaultValue = 1) {
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetInt(prefKey, defaultValue);
			return PlayerPrefs.GetInt(prefKey);
		}



		public static void SetString(string prefKey, string value) {
			PlayerPrefs.SetString(prefKey, value);
		}

		public static string LoadString(string prefKey, string defText) {
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetString(prefKey, defText);
			return PlayerPrefs.GetString(prefKey);
		}



		public static DateTime LoadDateTime(string prefKey) {
			DateTime dateTime = DateTime.Now;
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetString(prefKey, dateTime.ToString());
			string dateTimeString = PlayerPrefs.GetString(prefKey);
			try {
				dateTime = DateTime.Parse(dateTimeString);
			}
			catch {
				Debug.LogError("Can not parse date time. Returned NOW");
				dateTime = DateTime.Now;
			}
			return dateTime;
		}
	}
}
