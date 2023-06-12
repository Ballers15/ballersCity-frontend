using SinSity.Core;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI
{
    public class PanelIdleObjectActiveStateInfo : MonoBehaviour
    {
        [SerializeField]
        private PanelCollectedCurrencyLeaf panelCollectedLeaf;

        [SerializeField]
        private PanelCollectedCurrency panelCollectedCurrency;

        [SerializeField]
        private ProgressBarCollecting progressBarCollecting;

        private IdleObject idleObject;
        private IdleObjectState state;

        private const float PERIOD_TRASHOLD_TOO_FAST = 0.3f;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            idleObject = gameObject.GetComponentInParent<IdleObject>();
            idleObject.OnInitialized += OnIdleObjectInitialized;
        }

        private void OnCurrencyCollected(object sender, BigNumber bigNumber) {
            if (sender is IdleObject)
                panelCollectedLeaf.Collect(bigNumber);
        }

        private void OnIdleObjectInitialized()
        {
            idleObject.OnInitialized -= OnIdleObjectInitialized;
            state = idleObject.state;
            UpdateView();
        }

        private void UpdateView()
        {
            panelCollectedLeaf.SetActive(state.hasAnyCollectedCurrency);
            panelCollectedCurrency.SetValue(state.collectedCurrency);
        }

        private void OnEnable()
        {
            idleObject.OnStateChangedEvent += IdleObject_OnStateChanged;
            idleObject.OnCurrencyCollected += OnCurrencyCollected;
            if (idleObject.isInitialized)
                UpdateView();
        }

        private void IdleObject_OnStateChanged(IdleObjectState newstate)
        {
            if (!newstate.autoPlayEnabled)
            {
                progressBarCollecting.ActivateFillerDefault();
            }
            else if (newstate.incomePeriod <= PERIOD_TRASHOLD_TOO_FAST)
                progressBarCollecting.ActivateTooFastFiller();
            UpdateView();
        }

        private void OnDisable()
        {
            idleObject.OnStateChangedEvent -= IdleObject_OnStateChanged;
            idleObject.OnCurrencyCollected -= OnCurrencyCollected;
        }

        private void Update()
        {
            if (state == null || idleObject == null || !idleObject.isInitialized)
                return;

            progressBarCollecting.SetValue(state.progressNomalized);
        }
    }
}