using UnityEngine.EventSystems;

namespace Orego.Util
{
    public sealed class PointerEnterHandler : AutoMonoBehaviour, IPointerEnterHandler
    {
        #region Event

        public AutoEvent<PointerEventData> OnPointerEnterEvent { get; }

        public PointerEnterHandler()
        {
            OnPointerEnterEvent = this.AutoInstantiate<AutoEvent<PointerEventData>>();
        }

        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnPointerEnterEvent.Invoke(eventData);
        }
    }
}