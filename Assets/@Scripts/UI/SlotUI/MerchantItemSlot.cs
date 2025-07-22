using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantItemSlot : MonoBehaviour
{
    public event Action OnPurchaseItem;

    Sprite _itemImage;
    [SerializeField] TMP_Text _itemName;
    [SerializeField] TMP_Text _costText;
    [SerializeField] TMP_Text _noticeText;
    [SerializeField] Button _purchaseButton;
    ItemData _item;

    bool _isPurchased = false;

    public void SetMerchantItemSlot(ItemData item)
    {
        _isPurchased = false;
        _item = item;
        _itemImage = item.IconImage;
        transform.GetChild(0).GetComponent<Image>().sprite = _itemImage;
        _itemName.text = item.Name;
        _costText.text = item.Cost.ToString();

        CheckGem(PlayerManager.Instance.Player.Inventory.Goods[Define.GoodsType.Gem]);
    }

    public void CheckGem(int gem)
    {
        if (_item.Cost > gem)
        {
            _purchaseButton.interactable = false;
            _noticeText.text = "젬 부족";
        }
        else if(!_isPurchased)
        {
            _noticeText.text = "";
            _purchaseButton.interactable = true;
        }
    }

    private void Awake()
    {
        _purchaseButton.onClick.AddListener(PurchaseItem);
    }

    void PurchaseItem()
    {
        PlayerManager.Instance.Player.Inventory.UseGoods(Define.GoodsType.Gem, _item.Cost);
        _noticeText.enabled = true;
        _noticeText.text = "판매 완료";
        _purchaseButton.interactable = false;
        _isPurchased = true;
        OnPurchaseItem?.Invoke();

        //실제 인벤토리에 구매한 아이템 적용시키기
        PlayerManager.Instance.Player.Inventory.AddItem(_item, 1);
        PlayerManager.Instance.Player.Inventory.ApplyChangesToSO(
            PlayerManager.Instance.Player.PlayerInventoryData);
    }
}
