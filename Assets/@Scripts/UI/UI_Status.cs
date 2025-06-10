using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_Status : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] PlayerInventoryData _inventoryData;
    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] List<UI_StatusSlot> _statusList;
    [SerializeField] Button _exitButton;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _inventoryData.OnValueChanged += UpdateStatusUI;
    }

    private void OnDisable()
    {
        _inventoryData.OnValueChanged -= UpdateStatusUI;
    }

    void Initialize() {
        _silverCoinText.text = _inventoryData.silverCoin.ToString();
        foreach (var slot in _statusList)
        {
            slot.Initialize(_playerData, _inventoryData);
        }
        _exitButton.onClick.AddListener(OnExitButtonClick);
     }

    private void UpdateStatusUI(Define.GoodsType type)
    {
        if(type == Define.GoodsType.SilverCoin)
            _silverCoinText.text = _inventoryData.silverCoin.ToString();
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
