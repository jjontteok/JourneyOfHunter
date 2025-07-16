using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_QuestPanel : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] Button _getWholeRewardButton;
    [SerializeField] TMP_Text _wholeRewardsText;
    [SerializeField] List<UI_MissionSlot> _statusList;
    [SerializeField] List<Button> _getRewardButtonList;
    [SerializeField] Button _exitButton;

    PlayerController _player;
    Inventory _inventoryData;


    //private void Awake()
    //{
    //    Initialize();
    //}

    //private void OnEnable()
    //{
    //    _inventoryData.OnValueChanged += UpdateStatusUI;
    //    _silverCoinText.text = _inventoryData.Goods[Define.GoodsType.SilverCoin].ToString();
    //}

    //private void OnDisable()
    //{
    //    _inventoryData.OnValueChanged -= UpdateStatusUI;
    //}

    //void Initialize()
    //{
    //    _player = PlayerManager.Instance.Player;
    //    _inventoryData = _player.Inventory;
    //    _silverCoinText.text = _inventoryData.Goods[Define.GoodsType.SilverCoin].ToString();
    //    foreach (var slot in _statusList)
    //    {
    //        slot.Initialize(_player.PlayerData, _inventoryData);
    //    }
    //    _exitButton.onClick.AddListener(OnExitButtonClick);
    //}

    //private void UpdateStatusUI(Define.GoodsType type)
    //{
    //    if (type == Define.GoodsType.SilverCoin)
    //        _silverCoinText.text = _inventoryData.Goods[Define.GoodsType.SilverCoin].ToString();
    //}

    //void OnExitButtonClick()
    //{
    //    OnExitButtonClicked?.Invoke();
    //    AudioManager.Instance.PlayClickSound();
    //}
}
