using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MerchantItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public int Cost;

    public ItemEffect Effect; //사용 시 발동될 효과
}

public class PopupUI_Merchant : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] Button _exitButton;
    List<MerchantItemData> _itemList;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _itemList = new();
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnEnable()
    {
        SetItemList();
        // 상인 구매창 뜨면 가만히 서있도록
        PlayerManager.Instance.IsAutoMoving = false;
        FieldManager.Instance.IsClear = true;
    }

    void SetItemList()
    {
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }

    private void OnDisable()
    {
        PlayerManager.Instance.IsAutoMoving = true;
    }
}
