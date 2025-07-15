using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// * 랜덤 뽑기 패널 스크립트
public class PopupUI_RandomSummon : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _equipmentItemButton;
    [SerializeField] Button _skillItemButton;
    [SerializeField] Button _drawOneTimeEquipmentItemButton;
    [SerializeField] Button _drawTenTimeEquipmentItemButton;
    [SerializeField] Button _drawOneTimeSkillButton;
    [SerializeField] Button _drawTenTimeSkillButton;
    [SerializeField] GameObject _equipmentGachaPanel;
    [SerializeField] GameObject _skillGachaPanel;
    [SerializeField] GameObject _resultPanel;
    [SerializeField] Transform _resultPanelViewport;
    [SerializeField] Button _resultPanelExitButton;

    [SerializeField] ItemList _randomItems;
    [SerializeField] SkillItemList _randomSkillItems;

    public event Action OnExitButtonClicked;

    private RandomSummonService _randomSummonService;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _resultPanel.SetActive(false);
        OnEquipmentGachaPanel();
    }

    private void Initialize()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _equipmentItemButton.onClick.AddListener(OnEquipmentGachaPanel);
        _skillItemButton.onClick.AddListener(OnSkillGachaPanel);
        _resultPanelExitButton.onClick.AddListener(OnResultPanelExit);

        _drawOneTimeEquipmentItemButton.onClick.AddListener(OnClick_DrawEquipment_One);
        _drawTenTimeEquipmentItemButton.onClick.AddListener(OnClick_DrawEquipment_Ten);
        _drawOneTimeSkillButton.onClick.AddListener(OnClick_DrawSkill_One);
        _drawTenTimeSkillButton.onClick.AddListener(OnClick_DrawSkill_Ten);

        _randomSummonService = new RandomSummonService(_randomItems, _randomSkillItems);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }

    void OnEquipmentGachaPanel()
    {
        _equipmentGachaPanel.SetActive(true);
        _skillGachaPanel.SetActive(false);
    }

    void OnSkillGachaPanel()
    {
        _equipmentGachaPanel.SetActive(false);
        _skillGachaPanel.SetActive(true);
    }

    void OnResultPanelExit()
    {
        _resultPanel.SetActive(false);
    }

    public void OnClickDrawButton(int drawCount, Define.DrawItemType drawItemType)
    {
        _resultPanel.SetActive(true);
        Dictionary<Data, int> items;
        items = _randomSummonService.Draw(drawCount, drawItemType);

        switch (drawItemType)
        {
            case Define.DrawItemType.Equipment:
                PlayerManager.Instance.Player.Inventory.AddItem(items);
                break;
            case Define.DrawItemType.Skill:
                PlayerManager.Instance.SkillSystem.AddSkillItem(items);
                break;
            default:
                break;
        }
        SetResultPanel(items);
    }

    public void OnClick_DrawEquipment_One() => OnClickDrawButton(1, Define.DrawItemType.Equipment);
    public void OnClick_DrawEquipment_Ten() => OnClickDrawButton(10, Define.DrawItemType.Equipment);
    public void OnClick_DrawSkill_One() => OnClickDrawButton(1, Define.DrawItemType.Skill);
    public void OnClick_DrawSkill_Ten() => OnClickDrawButton(10, Define.DrawItemType.Skill);

    private void SetResultPanel(Dictionary<Data, int> items)
    {
        ClearSlots();
        PlayerManager.Instance.Player.Inventory.ApplyChangesToSO(PlayerManager.Instance.Player.PlayerInventoryData);
        foreach (var item in items)
        {
            if (item.Key is ItemData)
            {
                string slotName = ((ItemData)item.Key).Value switch
                {
                    Define.ItemValue.Common => "ItemSlot - Common",
                    Define.ItemValue.Uncommon => "ItemSlot - Uncommon",
                    Define.ItemValue.Rare => "ItemSlot - Rare",
                    Define.ItemValue.Epic => "ItemSlot - Epic",
                    Define.ItemValue.Legendary => "ItemSlot - Legendary",
                    _ => "ItemSlot - Normal"
                };
                for (int i = 0; i < item.Value; i++)
                {
                    GameObject itemSlot = PoolManager.Instance.GetObjectFromPool<ItemSlot>(Vector3.zero, slotName, _resultPanelViewport);
                    itemSlot.GetComponent<ItemSlot>().SetData(item.Key as ItemData);
                }
            }
            else if (item.Key is SkillData)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    GameObject skillSlot = PoolManager.Instance.GetObjectFromPool<SkillItemSlot>(Vector3.zero, "ItemSlot - Skill", _resultPanelViewport);
                    skillSlot.GetComponent<SkillItemSlot>().UpdateSlot(item.Key as SkillData, true);
                }
            }
        }
    }
    void ClearSlots()
    {
        for (int i = 0; i < _resultPanelViewport.childCount; i++)
        {
            GameObject slotObj = _resultPanelViewport.GetChild(i).gameObject;
            slotObj.SetActive(false); // 또는 PoolManager로 반환
        }
    }

    #region RandomSummonService
    // 내부 클래스 정의
    private class RandomSummonService
    {
        private ItemList _itemList;
        private SkillItemList _skillList;

        public RandomSummonService(ItemList itemList, SkillItemList skillItemList)
        {
            _itemList = itemList;
            _skillList = skillItemList;
        }

        public Dictionary<Data, int> Draw(int count, Define.DrawItemType type)
        {
            var result = new Dictionary<Data, int>();
            for (int i = 0; i < count; i++)
            {
                Data item = (type == Define.DrawItemType.Equipment)
                    ? _itemList.GetRandomItem()
                    : _skillList.GetRandomSkillItem();
                if (result.ContainsKey(item))
                {
                    result[item]++;
                }
                else
                {
                    result[item] = 1;
                }
            }
            return result;
        }
    }
    #endregion
}
