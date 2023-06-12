using System;
using UnityEngine;

namespace VavilichevGD {
	public class Number {

		public static readonly string[] numbNames = {
		"", "ID_THOUSAND", "ID_MILLION", "ID_BILLION", "ID_TRILLION", "aa", "bb",
		"cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "kk", "ll", "mm", "nn",
		"oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "ww", "xx", "yy", "zz",
	};

		public float count { get; private set; }
		public string name { get; private set; }

		private string prefKeyCount;
		private string prefKeyLevel { get { return "LEVEL_" + prefKeyCount; } }
		private int level;
		private bool saveEnabled;

		public Number(string _prefKey) {
			prefKeyCount = _prefKey;
			saveEnabled = true;

			Load();
		}

		public Number(float defCount, int defLevel) {
			level = defLevel;
			ApplyNewCount(defCount);
		}

		private void Load() {
			if (saveEnabled) {
				count = Loader.LoadFloat(prefKeyCount, 0f);
				level = Loader.LoadInteger(prefKeyLevel, 0);
				name = numbNames[level];
			}
		}

		public void AddPercent(int value) {
			if (value >= 0) {
				float normalizedValue = value / 100f;
				float newCount = count * (1 + normalizedValue);
				ApplyNewCount(newCount);
			}
			else 
				throw new Exception("Value can not be negative");
		}

		private void ApplyNewCount(float newCount) {
			float newCountRounded = (float)Math.Round(newCount, 1);
			count = newCountRounded;
			while(count > 1000)
				NextLevel(count);
			Save();
		}

		private void Add(float value) {
			if (value >= 0) {
				float newCount = count + value;
				ApplyNewCount(newCount);
			}
			else
				throw new Exception("Value can not be negative");
		}

		public void Add(Number numbValue) {

			Add(numbValue.count);
		}

		private void NextLevel(float newCountRounded) {
			level++;
			name = numbNames[level];

			float newCount = newCountRounded / 1000f;
			count = (float)Math.Round(newCount, 1);
		}

		private void Save() {
			if (saveEnabled) {
				PlayerPrefs.SetFloat(prefKeyCount, count);
				PlayerPrefs.SetInt(prefKeyLevel, level);
			}
		}

		public override string ToString() {
			return count + " " + name;
		}

		public void Reset() {
			if (saveEnabled) {
				PlayerPrefs.DeleteKey(prefKeyCount);
				PlayerPrefs.DeleteKey(prefKeyLevel);
				Load();
			}
		}
	}
}