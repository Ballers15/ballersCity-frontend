using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Repo;
using SinSity.Services;
using AnalyticsEvent = SinSity.Services.AnalyticsEvent;

public class ModernizationAnalytics {

	#region CONSTANTS

	private const string EVENT_NAME_MDRN_HAPPEND = "modernization_happend";
	private const string EVENT_NAME_MDRN_HINT = "modernization_hint_result";
	private const string EVENT_NAME_MDRN_POPUP = "modernization_popup_result";
	private const string PAR_NAME_NUMBER = "number";
	private const string PAR_NAME_RESULT = "result";
	public const string RESULT_VALUE_MDRN_HAPPEND = "modernization_happend";
	public const string RESULT_VALUE_CLOSED = "closed";

	#endregion

	public bool hintEnabled { get; set; }
	
	private ModernizationInteractor interactor;
	private ModernizationRepository repository;

	public ModernizationAnalytics(ModernizationInteractor interactor, ModernizationRepository repository) {
		this.hintEnabled = false;
		this.interactor = interactor;
		this.repository = repository;
		
		this.interactor.OnModernizationCompleteEvent += this.OnModernizationComplete;
	}
	
	public void LogModernizationPopupResults(string result) {
		var parameters = new Dictionary<string, object> {
			{PAR_NAME_RESULT, result}
		};

		var e = new AnalyticsEvent(EVENT_NAME_MDRN_POPUP, parameters);
		CommonAnalytics.Log(e);

		if (this.hintEnabled) 
			this.LogModernizationHintResults(result);
	}

	private void LogModernizationHappend(int number) {
		var parameters = new Dictionary<string, object> {
			{PAR_NAME_NUMBER, number}
		};

		var e = new AnalyticsEvent(EVENT_NAME_MDRN_HAPPEND, parameters);
		CommonAnalytics.Log(e);
	}

	private void LogModernizationHintResults(string result) {
		this.hintEnabled = false;
		
		var parameters = new Dictionary<string, object> {
			{PAR_NAME_RESULT, result}
		};

		var e = new AnalyticsEvent(EVENT_NAME_MDRN_HINT, parameters);
		CommonAnalytics.Log(e);
	}

	#region CALLBACKS

	private void OnModernizationComplete(ModernizationInteractor modernizationInteractor, object sender) {
		var modernizationNumber = this.repository.data.renovationIndex;
		this.LogModernizationHappend(modernizationNumber);
	}

	#endregion

}