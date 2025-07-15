using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantItemSlot : MonoBehaviour
{
    Sprite _itemImage;
    [SerializeField] TMP_Text _itemName;
    [SerializeField] TMP_Text _costText;
    [SerializeField] TMP_Text _noticeText;
    [SerializeField] Button _purchaseButton;

    public void SetMerchantItemSlot(ItemData item)
    {
        _itemImage = item.IconImage;
        transform.GetChild(0).GetComponent<Image>().sprite = _itemImage;
        _itemName.text = item.Name;
        _costText.text = item.Cost.ToString();

        if (item.Cost > PlayerManager.Instance.Player.Inventory.Goods[Define.GoodsType.Gem])
        {
            _purchaseButton.interactable = false;
            _noticeText.text = "젬 부족";
        }
        else
            _purchaseButton.interactable = true;
    }

    private void Awake()
    {
        _purchaseButton.onClick.AddListener(PurchaseItem);
    }

    void PurchaseItem()
    {
        _noticeText.enabled = true;
        _noticeText.text = "판매 완료";
        _purchaseButton.interactable = false;

        //실제 인벤토리에 구매한 아이템 적용시키기
    }
}
