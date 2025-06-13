using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupUI_Panel : MonoBehaviour, IPointerDownHandler
{
    public event Action OnPopupPanelClicked;
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPopupPanelClicked?.Invoke();
    }
}
