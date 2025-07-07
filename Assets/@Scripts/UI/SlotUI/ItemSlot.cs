using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 아이템 슬롯 클래스
[RequireComponent(typeof(EventTrigger))]
public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float longPressedTime = 0.5f;

    private Coroutine _longPressCoroutine;
    private Vector2 _toolTipOffSet = new Vector2(150, 100);

    [SerializeField] Sprite _itemImage;

    ItemData _itemData;

    // 아이템 슬롯 초기화 필수 메서드
    public void SetData(ItemData itemData)
    {
        _itemData = itemData;
        _itemImage = _itemData.IconImage;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = _itemImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _longPressCoroutine = StartCoroutine(CheckLongPress(eventData.position));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopLongPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopLongPress();
    }

    private IEnumerator CheckLongPress(Vector2 touchPos)
    {
        yield return new WaitForSeconds(longPressedTime);
        OnToolTip(touchPos);
    }

    // 나영님이 Define으로 색 딕셔너리 옮기시면 아이템 슬롯의 아이템 데이터 별 색으로 4번째 매개변수 전달하기! -> 툴팁의 이름 등급별 색상 변경 적용
    private void OnToolTip(Vector2 touchPos)
    {
        PopupUIManager.Instance.ActivateToolTipPanel(touchPos + _toolTipOffSet, _itemData.Name, _itemData.Description);
    }

    private void OffToolTip()
    {
        PopupUIManager.Instance.DeactivateToolTipPanel();
    }

    private void StopLongPress()
    {
        if (_longPressCoroutine != null)
        {
            StopCoroutine(_longPressCoroutine);
            _longPressCoroutine = null;
        }
        OffToolTip();
    }
}
