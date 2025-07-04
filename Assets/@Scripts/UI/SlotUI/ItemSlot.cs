using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 아이템 슬롯 클래스
[RequireComponent(typeof(EventTrigger))]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] Sprite _itemImage; 

    ItemData _itemData;

    public void SetData(ItemData itemData)
    {
        _itemData = itemData;
        _itemImage = _itemData.IconImage;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = _itemImage;
    }
}
