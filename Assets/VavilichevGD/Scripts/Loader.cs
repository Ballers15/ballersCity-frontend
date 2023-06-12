using System;
using UnityEngine;

public class Loader {

	public static bool LoadBool(string prefKey, bool defaultValue = true) {
		if (!PlayerPrefs.HasKey(prefKey))
			PlayerPrefs.SetInt(prefKey, BoolToInteger(defaultValue));
		int intValue = PlayerPrefs.GetInt(prefKey);
		return IntToBool(intValue);
	}

	private static int BoolToInteger(bool value) {
		if (value)
			return 1;
		return 0;
	}

	private static bool IntToBool(int value) {
		return value != 0;
	}

	public static float LoadFloat(string prefKey, float defaultValue = 1f) {
		if (!PlayerPrefs.HasKey(prefKey))
			PlayerPrefs.SetFloat(prefKey, defaultValue);
		return PlayerPrefs.GetFloat(prefKey);
	}

	public static int LoadInteger(string prefKey, int defaultValue = 1) {
		if (!PlayerPrefs.HasKey(prefKey))
			PlayerPrefs.SetInt(prefKey, defaultValue);
		return PlayerPrefs.GetInt(prefKey);
	}

	public static void SetInteger(string prefKey, int defaultValue = 0) {
		PlayerPrefs.SetInt(prefKey, defaultValue);
	}

	public static void SetBool(string prefKey, bool value) {
		PlayerPrefs.SetInt(prefKey, BoolToInteger(value));
	}

	public static T LoadEnum<T>(string prefKey, Enum defaultValue) where T : Enum {
		if (!PlayerPrefs.HasKey(prefKey))
			PlayerPrefs.SetString(prefKey, defaultValue.ToString());
		string strValue = PlayerPrefs.GetString(prefKey);
		var value = Enum.Parse(typeof(T), strValue);
		return (T)value;
	}

	public static void SetEnum(string prefKey, Enum value) {
		PlayerPrefs.SetString(prefKey, value.ToString());
	}

	public static string LoadString(string prefKey, string defaultValue = "") {
		if (!PlayerPrefs.HasKey(prefKey))
			PlayerPrefs.SetString(prefKey, defaultValue);
		return PlayerPrefs.GetString(prefKey);
	}

	public static void SetString(string prefKey, string value) {
		PlayerPrefs.SetString(prefKey, value);
	}

	public static void Clean(string prefKey) {
		PlayerPrefs.DeleteKey(prefKey);
		Debug.Log(string.Format("PrefKey {0} deleted", prefKey));
	}
}
