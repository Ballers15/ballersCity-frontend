/* This script is for working with time. It can load time from server, notification about results, 
 * and save and load DateTime in PlayerPrefs with prefKey (static methods)*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace VavilichevGD {
	public class TimeManager {

		[Serializable]
		public class Properties {
			public string url = "https://andreyvavilichev.000webhostapp.com/GetTime.php";
			public int breakTime = 3;
			public bool debugMode;
			public MonoBehaviour mono { get; set; }
		}

		public struct DownloadTimeArgs {
			public bool success;
			public string error;
			public DateTime dateTime;

			public DownloadTimeArgs(bool _success, string _error, DateTime _dateTime) {
				success = _success;
				error = _error;
				dateTime = _dateTime;
			}
		}

		private Coroutine routine;
		private Properties properties;

		public delegate void DownloadTimeHandler(DownloadTimeArgs e);
		public event DownloadTimeHandler OnTimeDownloaded;

		public TimeManager(MonoBehaviour _mono, Properties _properties) {
			properties = _properties;
			properties.mono = _mono;
		}

		public void LoadFromInternet() {
			StopRoutine();
			routine = properties.mono.StartCoroutine(LoadTimeRoutine());
		}

		private void StopRoutine() {
			if (routine != null) {
				properties.mono.StopCoroutine(routine);
				routine = null;
			}
		}

		private IEnumerator LoadTimeRoutine() {
			string url = properties.url;
			Log(string.Format("Start downloading time from the servevr: {0}", url));

			UnityWebRequest request = new UnityWebRequest(url);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.timeout = properties.breakTime;

			yield return request.SendWebRequest();
			if (IsError(request))
				yield break;

			string stringDateTime = request.downloadHandler.text;
			DateTime dateTime = ParseData(stringDateTime);
			Log(string.Format("TimeManager: <color=green>SUCCESS</color> Downloaded Date: {0}", dateTime.ToString()));
			NotifyAboutDownloadingResults(true, "", dateTime);
		}

		private DateTime ParseData(string stringDateTime) {
			string[] words = stringDateTime.Split('/');
			if (words.Length == 2) {
				int day = 0, mon = 0, year = 0, hour = 0, min = 0, sec = 0;

				string[] numbersLeft = words[0].Split('.');
				if (numbersLeft.Length == 3) {
					string sDay = numbersLeft[0];
					day = Convert.ToInt32(sDay);
					string sMonth = numbersLeft[1];
					mon = Convert.ToInt32(sMonth);
					string sYear = numbersLeft[2];
					year = Convert.ToInt32(sYear);
				}

				string[] numbersRight = words[1].Split(':');
				if (numbersRight.Length == 3) {
					string sHour = numbersRight[0];
					hour = Convert.ToInt32(sHour);
					string sMin = numbersRight[1];
					min = Convert.ToInt32(sMin);
					string sSec = numbersRight[2];
					sec = Convert.ToInt32(sSec);
				}

				DateTime dateTime = new DateTime(year, mon, day, hour, min, sec);
				return dateTime;
			}
			return new DateTime();
		}

		private bool IsError(UnityWebRequest request) {
			string error = "";
			 
			if (request.isNetworkError)
				error = string.Format("<color=red>Error:</color> Downloading time stopped: {0}", request.error);
			else if (request.downloadHandler == null)
				error = string.Format("<color=red>Error:</color> Downloading time stopped: DownloadHandler is NULL");
			else if (string.IsNullOrEmpty(request.downloadHandler.text))
				error = string.Format("<color=red>Error:</color> Downloading time stopped: Downloaded string is empty or NULL");

			if (string.IsNullOrEmpty(error))
				return false;

			Log(error);
			NotifyAboutDownloadingResults(false, error, new DateTime());
			return true;
		}

		private void NotifyAboutDownloadingResults(bool success, string error, DateTime dateTime) {
			if (OnTimeDownloaded != null) {
				DownloadTimeArgs args = new DownloadTimeArgs(success, error, dateTime);
				OnTimeDownloaded(args);
			}
		}

		private void Log(string text) {
			if (properties.debugMode)
				Debug.Log(text);
		}

		public static void SaveTime(string prefKey, DateTime dateTime) {
			PlayerPrefs.SetString(prefKey, dateTime.ToString());
		}

		public static DateTime LoadTime(string prefKey) {
			if (!PlayerPrefs.HasKey(prefKey))
				PlayerPrefs.SetString(prefKey, new DateTime().ToString());
			string strDateTime = PlayerPrefs.GetString(prefKey);
			DateTime dateTime = DateTime.Parse(strDateTime);
			return dateTime;
		}
	}
}

