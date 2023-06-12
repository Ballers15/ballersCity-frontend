using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VavilichevGD.LocalizationFramework {
	public static class LocalizationParser {

		private const string REGEX_MASK = "\"([^\"]*)\"";

		public static Dictionary<string, string> Parse(string tableText) {
			Dictionary<string, string> entities = new Dictionary<string, string>();
			List<string> allWords = GetWordsFromText(tableText);
			for (int i = 0; i < allWords.Count; i += 2) {
				string key = allWords[i];
				string value = allWords[i + 1];
				if (entities.ContainsKey(key))
					throw new Exception($"Dictionary already has key: {key}");
				entities.Add(key, value);
			}
			return entities;
		}

		private static List<string> GetWordsFromText(string text) {
			if (HasOnlyTwoColumns(text)) {
				List<string> words = new List<string>();
				foreach (Match match in Regex.Matches(text, REGEX_MASK))
					words.Add(match.ToString().Replace("\"", ""));
				return words;
			}
			throw new System.Exception($"Table must contain two columns: ID and VALUE. Table: {text}");
		}

		private static bool HasOnlyTwoColumns(string tableText) {
			string firstLine = tableText.Split('\n')[0];
			int columnsCount = Regex.Matches(firstLine, REGEX_MASK).Count;
			return columnsCount == 2;
		}
	}
}