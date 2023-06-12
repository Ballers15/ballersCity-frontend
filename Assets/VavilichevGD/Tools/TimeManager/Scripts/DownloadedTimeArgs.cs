using System;

namespace VavilichevGD.Tools {
	public struct DownloadedTimeArgs {
		public DateTime downloadedTime { get; }
		public bool error { get; }
		public string errorText { get; }
		public bool loadedFromServer { get; }

		public DownloadedTimeArgs(DateTime _dowloadedTime, bool _error, string _errorText, bool _loadedFromServer) {
			downloadedTime = _dowloadedTime;
			error = _error;
			errorText = _errorText;
			loadedFromServer = _loadedFromServer;
		}
	}
}