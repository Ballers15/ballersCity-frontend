using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace VavilichevGD.Tools {
	public class TimeLoader {

		public bool isLoading { get; private set; }

		private const bool LOADED_FROM_LOCAL = false;
		private const bool LOADED_FROM_INTERNET = true;
		private const int BREAK_TIME_DEFAULT = 2;
		private const string SERVER_URL = "https://www.microsoft.com";

		public delegate void DownloadTimeHandler(TimeLoader timeLoader, DownloadedTimeArgs e);
		public event DownloadTimeHandler OnTimeDownloaded;

		public Coroutine LoadTime(int breakTime = BREAK_TIME_DEFAULT) {
			if (!isLoading)
				return Coroutines.StartRoutine(LoadTimeRoutine(breakTime));
			return null;
		}

		private IEnumerator LoadTimeRoutine(int breakTime) {
			isLoading = true;

			UnityWebRequest request = new UnityWebRequest(SERVER_URL);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.timeout = breakTime;

			yield return request.SendWebRequest();
			if (NotValidResponse(request))
				yield break;
			
			var todaysDates = request.GetResponseHeaders()["date"];
			DateTime downloadedTime = DateTime.ParseExact(todaysDates,
									   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
									   CultureInfo.InvariantCulture.DateTimeFormat,
									   DateTimeStyles.AdjustToUniversal);

			NotifyAboutDownloadedTime(downloadedTime, false, null, LOADED_FROM_INTERNET);
			isLoading = false;
		}

		private bool NotValidResponse(UnityWebRequest request) {
			string errorText = "";

			if (request.isNetworkError)
				errorText = $"Downloading time stopped: {request.error}";
			else if (request.downloadHandler == null)
				errorText = $"Downloading time stopped: DownloadHandler is NULL";
			else if (string.IsNullOrEmpty(request.downloadHandler.text))
				errorText = $"Downloading time stopped: Downloaded string is empty or NULL";

			if (string.IsNullOrEmpty(errorText))
				return false;

			NotifyAboutDownloadedTime(new DateTime(), true, errorText, LOADED_FROM_LOCAL);
			return true;
		}

		private void NotifyAboutDownloadedTime(DateTime downloadedTime, bool error, string errorText, bool downloadedFromServer) {
			DownloadedTimeArgs downloadedTimeArgs = new DownloadedTimeArgs(downloadedTime, error, errorText, downloadedFromServer);
			OnTimeDownloaded?.Invoke(this, downloadedTimeArgs);
		}
	}
}