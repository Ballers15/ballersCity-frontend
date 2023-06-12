using SinSity.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SinSity.UI
{
    public class IdleObjectCollectButton : MonoBehaviour, IPointerEnterHandler
    {
        private IdleObject idleObject;

        private void Awake()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.idleObject = this.gameObject.GetComponentInParent<IdleObject>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.idleObject.CollectCurrency();
        }
    }
}