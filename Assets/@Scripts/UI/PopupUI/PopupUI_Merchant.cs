using extension;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PopupUI_Merchant : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] Button _exitButton;
    [SerializeField] ItemList _merchantAllItems;
    [SerializeField] TMP_Text _gemText;
    List<MerchantItemSlot> _merchantItemSlots; //상인 아이템 프리팹 저장 리스트

    Dictionary<ItemData, int> _currentitemList; //UI에 보여질 아이템 리스트

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _merchantItemSlots = new List<MerchantItemSlot>();
        _currentitemList = new Dictionary<ItemData, int>();
        _exitButton.onClick.AddListener(OnExitButtonClick);

        for(int i = 0; i<4; i++)
        {
            GameObject merchantItemSlot = Instantiate(ObjectManager.Instance.MerchantItemSlot);
            _merchantItemSlots.Add(merchantItemSlot.GetOrAddComponent<MerchantItemSlot>());

            GameObject content = GetComponentInChildren<GridLayoutGroup>().gameObject;
            merchantItemSlot.transform.SetParent(content.transform);
        }
    }

    private void OnEnable()
    {
        PlayerManager.Instance.IsAutoMoving = false;
        FieldManager.Instance.IsClear = true;
        DrawMerchantItem();
    }

    public void SetItemList() {
        _gemText.text = PlayerManager.Instance.Player.Inventory.Goods[Define.GoodsType.Gem].ToString();
        _currentitemList.Clear();

        for (int i =0; i<4;)
        {
            ItemData item = _merchantAllItems.GetRandomItem();
            if (!_currentitemList.ContainsKey(item))
            {
                _currentitemList[item] = 1;
                 _merchantItemSlots[i].SetMerchantItemSlot(item);
                i++;
            }
        }
    }

    void DrawMerchantItem()
    {
        foreach(var item in _currentitemList)
        {
            
        }
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
        AudioManager.Instance.PlayClickSound();
    }

    private void OnDisable()
    {
        PlayerManager.Instance.IsAutoMoving = true;
    }
}
