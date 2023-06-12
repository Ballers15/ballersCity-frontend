using System;
using System.Collections.Generic;
using System.Linq;
using IdleClicker;
using SinSity.Domain;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core {
	public class BluePrint : MonoBehaviour {

		[SerializeField] private Transform[] containerZones;
		public static bool bluePrintModeEnabled { get; private set; }

		public delegate void BluePrintStateHandler(bool isActive);
		public static event BluePrintStateHandler OnBluePrintStateChanged;

		
		private const string BLUEPRINTSCHEME_PREFABS_FOLDER = "BluePrintSchemePrefabs/";

		private Dictionary<string, string> bluePrintSchemeNamesMap;
		private Dictionary<string, BluePrintScheme> createdBluePrintSchemesMap;
		private IdleObjectsUpgradeForAdInteractor idleObjectsInteractor;
		
		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			Game.OnGameInitialized += OnGameInitialized;
			CameraController.OnCameraMovingStart += OnCameraMovingStart;
			CameraController.OnCameraMovingOver += OnCameraMovingOver;
			UIWidgetBtnNavigateUpgrade.OnClicked += ButtonBluePrint_OnClicked;

			InitBluePrintSchemeNamesMap();
			Deactivate();
		}

        private void OnEnable()
        {
			idleObjectsInteractor.RefreshData();
		}

        private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= OnGameInitialized;
			bool allCreated = CreateDirtSlotsIfNecessary();
			if (!allCreated)
			{
				IdleObject.OnIdleObjectBuilt += OnIdleObjectBuilt;
			}
			
			idleObjectsInteractor = Game.GetInteractor<IdleObjectsUpgradeForAdInteractor>();
		}
		
		private bool CreateDirtSlotsIfNecessary() {
			createdBluePrintSchemesMap = new Dictionary<string, BluePrintScheme>();
            
			Zone[] zones = FindObjectsOfType<Zone>();
			IdleObject[] idleObjectsNew = GetIdleObjects(zones);
            
			bool allCreated = true;
			foreach (IdleObject idleObjectNew in idleObjectsNew) {
				if (idleObjectNew.isBuilt && bluePrintSchemeNamesMap.ContainsKey(idleObjectNew.id)) {
					string bluePrintName = bluePrintSchemeNamesMap[idleObjectNew.id];
					CreateScheme(bluePrintName);
				}
				else
					allCreated = false;
			}
			return allCreated;
		}
		
		private IdleObject[] GetIdleObjects(Zone[] zones) {
			List<IdleObject> idleObjectNewList = new List<IdleObject>();
			foreach (Zone zone in zones)
				idleObjectNewList.AddRange(zone.idleObjects);
			return idleObjectNewList.ToArray();
		}
		
		private void CreateScheme(string schemeName) {
			BluePrintScheme schemePref = Resources.Load<BluePrintScheme>($"{BLUEPRINTSCHEME_PREFABS_FOLDER}{schemeName}");
            
			Transform container = null;
			string containerName = schemePref.zoneName;
			foreach (Transform containerZone in containerZones) {
				if (containerZone.name.Equals(containerName))
					container = containerZone;
			}

			BluePrintScheme createdScheme = Instantiate(schemePref, container, true);
			var idleObjectId = createdScheme.idleObjectId;
			if (this.createdBluePrintSchemesMap.ContainsKey(idleObjectId))
			{
				Destroy(this.createdBluePrintSchemesMap[idleObjectId].gameObject);
				this.createdBluePrintSchemesMap.Remove(idleObjectId);
			}
			
			createdBluePrintSchemesMap.Add(idleObjectId, createdScheme);
			Resources.UnloadUnusedAssets();
		}
		
		private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state) {
			if (bluePrintSchemeNamesMap.ContainsKey(idleobject.id)) {
				string schemeName = bluePrintSchemeNamesMap[idleobject.id];
				CreateScheme(schemeName);
			}
		}
		
		private void OnCameraMovingStart(int fromPointIntex, int toPointIndex) {
			containerZones[toPointIndex].gameObject.SetActive(true);
		}
		
		private void OnCameraMovingOver(int fromPointIntex, int toPointIndex) {
			containerZones[fromPointIntex].gameObject.SetActive(false);
		}
		
		
		
		private void ButtonBluePrint_OnClicked() {
			bluePrintModeEnabled = !bluePrintModeEnabled;
			
			if (bluePrintModeEnabled)
				Activate();
			
			OnBluePrintStateChanged?.Invoke(bluePrintModeEnabled);
			SetActiveBackground(bluePrintModeEnabled);
			
//			if (!bluePrintModeEnabled)
//				InputManager.OnBackKeyClicked -= OnBackKeyClicked;
			
			UIController.NotifyAboutStateChanged();
		}
		
		private void Activate() {
			UpdateZones();
			gameObject.SetActive(true);
			CommonAnalytics.LogBluePrintTabOpened();
//			InputManager.OnBackKeyClicked += OnBackKeyClicked;
		}

//		private void OnBackKeyClicked() {
//			ButtonBluePrint_OnClicked();
//		}

		private void UpdateZones() {
			int indexCurrent = CameraController.instance.indexPointCurrent;
			int count = containerZones.Length;
			for (int i = 0; i < count; i++) {
				bool isActive = indexCurrent == i;
				containerZones[i].gameObject.SetActive(isActive);
			}
		}
		
		private void SetActiveBackground(bool isActive) {
			gameObject.SetActive(isActive);
		}

		private void InitBluePrintSchemeNamesMap() {
			BluePrintScheme[] bluePrintSchemes = Resources.LoadAll<BluePrintScheme>(BLUEPRINTSCHEME_PREFABS_FOLDER);
			bluePrintSchemeNamesMap = new Dictionary<string, string>();

			foreach (BluePrintScheme bluePrintScheme in bluePrintSchemes)
				bluePrintSchemeNamesMap.Add(bluePrintScheme.idleObjectId, bluePrintScheme.name);
			Resources.UnloadUnusedAssets();
		}
		
		private void Deactivate() {
			gameObject.SetActive(false);
		}
		
		private void OnDestroy() {
			UIWidgetBtnNavigateUpgrade.OnClicked -= ButtonBluePrint_OnClicked;
			CameraController.OnCameraMovingStart -= OnCameraMovingStart;
			CameraController.OnCameraMovingOver -= OnCameraMovingOver;
			UIWidgetBtnNavigateUpgrade.OnClicked -= ButtonBluePrint_OnClicked;
		}
	}
}