using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_ItemInfo : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _applyButton;
    [SerializeField] Button _takeOffButton;

    [SerializeField] Image _itemImage;
    [SerializeField] TMP_Text _name;
    [SerializeField] TMP_Text _value;
    [SerializeField] TMP_Text _type;

    [SerializeField] GameObject _itemOptionsPanel;
    [SerializeField] GameObject _itemDescriptionPanel;

    [SerializeField] TMP_Text _atkStatus;
    [SerializeField] TMP_Text _defStatus;
    [SerializeField] TMP_Text _hpStatus;
    [SerializeField] TMP_Text _hpRecStatus;
    [SerializeField] TMP_Text _coolTimeReduceStatus;
    [SerializeField] TMP_Text _speedStatus;
    [SerializeField] TMP_Text _itemDescription;

    ItemSlot _selectedItem;
    ItemData _itemData;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _exitButton.onClick.AddListener(Deactivate);

        _applyButton.onClick.AddListener(ApplyItem);
        _applyButton.onClick.AddListener(Deactivate);

        _takeOffButton.onClick.AddListener(SubtractItem);
        _takeOffButton.onClick.AddListener(Deactivate);
    }

    public void SetItemInfo(ItemSlot item)
    {
        _selectedItem = item;
        _itemData = _selectedItem.ItemData;

        _itemImage.sprite = _itemData.IconImage;
        _name.text = _itemData.Name;
        _value.text = _itemData.Value switch
        {
            Define.ItemValue.Common => "Common",
            Define.ItemValue.Uncommon => "Uncommon",
            Define.ItemValue.Rare => "Rare",
            Define.ItemValue.Epic => "Epic",
            Define.ItemValue.Legendary => "Legendary",
            _ => "None"
        };
        if (_itemData.Type == Define.ItemType.Equipment)
        {
            _itemDescriptionPanel.SetActive(false);
            _itemOptionsPanel.SetActive(true);
            _type.text = ((EquipmentItemData)_itemData).EquipmentType switch
            {
                Define.EquipmentItemType.Weapon => "무기",
                Define.EquipmentItemType.Helmet => "투구",
                Define.EquipmentItemType.Gloves => "장갑",
                Define.EquipmentItemType.Shoes => "신발",
                Define.EquipmentItemType.Armor => "갑옷",
                _ => "None"
            };

            _atkStatus.text = "공격력 : " + ((EquipmentItemData)_itemData).ItemStatus.Atk;
            _defStatus.text = "방어력 : " + ((EquipmentItemData)_itemData).ItemStatus.Def;
            _hpStatus.text = "체력 : " + ((EquipmentItemData)_itemData).ItemStatus.HP;
            _hpRecStatus.text = "체력 회복 : " + ((EquipmentItemData)_itemData).ItemStatus.HPRecoveryPerSec;
            _coolTimeReduceStatus.text = "쿨타임 감소 : " + ((EquipmentItemData)_itemData).ItemStatus.CoolTimeDecrease;
            _speedStatus.text = "이동 속도 : " + ((EquipmentItemData)_itemData).ItemStatus.Speed;

            _takeOffButton.gameObject.SetActive(true);
            _applyButton.gameObject.SetActive(true);

            _applyButton.GetComponentInChildren<TMP_Text>().text = "장착";
            _takeOffButton.GetComponentInChildren<TMP_Text>().text = "해제";
        }
        else if (_itemData.Type == Define.ItemType.Consumable)
        {
            _itemDescriptionPanel.SetActive(true);
            _itemOptionsPanel.SetActive(false);
            _type.text = "소비 아이템";

            _itemDescription.text = _itemData.Description;

            _applyButton.gameObject.SetActive(true);

            _applyButton.GetComponentInChildren<TMP_Text>().text = "사용";
            _takeOffButton.gameObject.SetActive(false);
        }
        else
        {
            _itemDescriptionPanel.SetActive(true);
            _itemOptionsPanel.SetActive(false);
            _type.text = "기타 아이템";

            _itemDescription.text = _itemData.Description;

            _applyButton.gameObject.SetActive(false);
            _takeOffButton.gameObject.SetActive(false);
        }
        _name.color = Define.EffectColorList[_itemData.Value];
        _value.color = Define.EffectColorList[_itemData.Value];
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void ApplyItem()
    {
        if(_itemData.Type == Define.ItemType.Equipment)
        {
            PopupUIManager.Instance.PanelInventory.GetComponent<PopupUI_Inventory>().EquipItem(_selectedItem);
        }
        else
        {
            PopupUIManager.Instance.PanelInventory.GetComponent<PopupUI_Inventory>().UseItem(_selectedItem);
        }
    }

    private void SubtractItem()
    {
        PopupUIManager.Instance.PanelInventory.GetComponent<PopupUI_Inventory>().UnEquipItem(_selectedItem);
    }
}
