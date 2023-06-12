using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Core {
    [CreateAssetMenu(fileName = "IdleObjectInfo", menuName = "IdleObjectInfo")]
    public class IdleObjectInfo : ScriptableObject {
        [SerializeField] protected string m_id;
        [SerializeField] protected int m_number;
        [Header("Price properties:")]
        [SerializeField] protected bool m_freeStart;
        [SerializeField] protected BigNumber m_priceToBuild;
        [SerializeField] protected BigNumber m_priceImproveDefault;
        [Range(1.07f, 1.15f)]
        [SerializeField] protected float m_priceStep;
        [Header("Income properties:")]
        [SerializeField] protected BigNumber m_incomeDefault;
        [SerializeField] protected float m_incomePeriodDefault;
        [Header("Other properties:")] 
        [SerializeField] protected Sprite m_spriteIcon;
        [SerializeField] protected Sprite m_productIcon;
        [SerializeField] protected string m_titleCode;
        [SerializeField] protected string m_descriptionCode;

        public string id
        {
            get => m_id;
            set => m_id = value;
        }

        public int number => m_number;
        public bool freeStart => m_freeStart;
        public Sprite productIcon => m_productIcon;
        
        public BigNumber priceToBuild
        { 
            get => freeStart ? new BigNumber(0) : m_priceToBuild;
            set => m_priceToBuild = value;
        }

        public BigNumber priceImproveDefault
        {
            get => m_priceImproveDefault;
            set => m_priceImproveDefault = value;
        }
        
        public float priceStep
        {
            get => m_priceStep;
            set => m_priceStep = value;
        }

        public BigNumber incomeDefault
        {
            get => m_incomeDefault;
            set => m_incomeDefault = value;
        }

        public float incomePeriodDefault
        {
            get => m_incomePeriodDefault;
            set => m_incomePeriodDefault = value;
        }

        public Sprite spriteIcon => m_spriteIcon;

        public string titleCode
        {
            get => m_titleCode;
            set => m_titleCode = value;
        }
        
        public string descriptionCode
        {
            get => m_descriptionCode;
            set => m_descriptionCode = value;
        }

        public const int MAX_LEVEL = 1000;

        public IdleObjectInfo()
        {
            m_priceToBuild = new BigNumber(0);
            m_priceImproveDefault = new BigNumber(0);
            m_incomeDefault = new BigNumber(0);
        }

        public virtual IdleObjectState CreateState(string stateJson) {
            IdleObjectState state = new IdleObjectState(this);
            state.Set(stateJson);
            return state;
        }
        
        #if UNITY_EDITOR
        public void SetupConfig(IdleObjectInfo info)
        {
            this.priceToBuild = info.priceToBuild;
            this.priceImproveDefault = info.priceImproveDefault;
            this.priceStep = info.priceStep;
            this.incomeDefault = info.incomeDefault;
            this.incomePeriodDefault = info.incomePeriodDefault;
            this.titleCode = info.titleCode;
            this.descriptionCode = info.descriptionCode;
        }
        #endif

        public string GetTitle() {
            return m_titleCode;
        }

        public string GetDescription() {
            return m_descriptionCode;
        }
    }
}