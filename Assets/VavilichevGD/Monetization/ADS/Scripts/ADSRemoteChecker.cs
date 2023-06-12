using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
	public class ADSRemoteChecker {

        #region CONSTANTS

        private const string SHEETS_ID = "1TRok7z3_38KKQZZ5vNXguSVRGS_-f9C-SltLC1b4818";

        #endregion

        public bool isAvailable { get; private set; }


        public Coroutine CheckAdAvailability() {
	        return Coroutines.StartRoutine(this.CheckAdAvailabilityRoutine());
        }

        private IEnumerator CheckAdAvailabilityRoutine() {
	        this.isAvailable = true;
	        const string url = @"https://docs.google.com/spreadsheet/ccc?key=1TRok7z3_38KKQZZ5vNXguSVRGS_-f9C-SltLC1b4818&usp=sharing&output=csv";
	        var request = UnityWebRequest.Get(url);
	        request.timeout = 1;
	        request.SendWebRequest();

	        while (!request.isDone)
		        yield return null;

	        if (request.downloadHandler != null && request.downloadHandler.text == "false")
		        this.isAvailable = false;
        }        
	}
}