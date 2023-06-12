using UnityEngine;
using UnityEngine.EventSystems;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverPanel;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(hoverPanel == null)
        {
            return;
        }

        hoverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverPanel == null)
        {
            return;
        }

        hoverPanel.SetActive(false);
    }
}
