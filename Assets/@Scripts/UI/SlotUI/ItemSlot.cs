using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 아이템 슬롯 클래스
[RequireComponent(typeof(EventTrigger))]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image _itemImage; 

    ItemData _itemData;
}
