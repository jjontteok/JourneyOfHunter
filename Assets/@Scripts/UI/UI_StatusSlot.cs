using extension;
using System;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_StatusSlot : MonoBehaviour
{
    public Button upgradeButton;

    [SerializeField] StatusSlotData _statusSlotData;
    [SerializeField] TMP_Text _statusNameText;
    [SerializeField] TMP_Text _currentStatusText;
    [SerializeField] TMP_Text _upgradeCostText;
    [SerializeField] Define.StatusType _statusType;

    EventTrigger _eventTrigger;
    PlayerData _playerData;
    Inventory _inventoryData;

    bool _isButtonDown = false;
    bool _isSilverCoinLack = false;

    float _time = 0;
    float _delay = 0.3f;

    private void Awake()
    {
        SetButtonEventTrigger();
    }
    void SetButtonEventTrigger()
    {
        _eventTrigger = upgradeButton.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((eventData) =>
        {
            if (!_isSilverCoinLack)
            {
                _isButtonDown = true;
                UpgradeStatus();
            }
        });

        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((eventData) => { _isButtonDown = false; });

        _eventTrigger.triggers.Add(entryPointerDown);
        _eventTrigger.triggers.Add(entryPointerUp);
    }

    private void OnEnable()
    {
        SetStatus();
    }


    public void Initialize(PlayerData playerData, Inventory inventoryData)
    {
        _playerData = playerData;
        _inventoryData = inventoryData;
    }

    void SetStatus()
    {
        _upgradeCostText.text = _statusSlotData.upgradeCost.ToString();
        float status=0;
        //일단 구현해
        switch (_statusType)
        {
            case Define.StatusType.Atk:
                status = _playerData.Atk; break;
            case Define.StatusType.Def:
                status = _playerData.Def; break;
            case Define.StatusType.HP:
                status = _playerData.HP; break;
            case Define.StatusType.HPRecoveryPerSec:
                status = _playerData.HPRecoveryPerSec; break;

        }
        _currentStatusText.text = status.ToString();
    }


    private void Update()
    {
        UpgradeButton();
        CheckUpgradeButton();
    }

    void UpgradeButton()
    {
        if (_isButtonDown && !_isSilverCoinLack)
        {
            _time += Time.deltaTime;

            if (_time > _delay)
            {
                UpgradeStatus();
                _time = 0;
                _delay = Mathf.Max(_delay * 0.9f, 0.05f);
            }
        }
        else
            _delay = 0.3f;
    }

    void CheckUpgradeButton()
    {
        upgradeButton.interactable =
            _statusSlotData.upgradeCost > _inventoryData.Goods[Define.GoodsType.SilverCoin] ?
            false : true;
        _isSilverCoinLack = upgradeButton.interactable ? false : true;
    }


    //스탯 업그레이드
    //근데 업글 비용과 스탯 수?를 정해야 한다
    public void UpgradeStatus()
    {
        _inventoryData.UseGoods(Define.GoodsType.SilverCoin, _statusSlotData.upgradeCost);
        _statusSlotData.level++;
        _statusSlotData.upgradeCost += 10 * _statusSlotData.level;

        _statusSlotData.currentStatusCount = (int)ApplyUpgrade(_statusSlotData.statusType, 5);
        _upgradeCostText.text = _statusSlotData.upgradeCost.ToString();
        _currentStatusText.text = _statusSlotData.currentStatusCount.ToString();

    }

    float ApplyUpgrade(Define.StatusType type, int amount)
    {
        float finalValue = 0;
        switch (type)
        {
            case Define.StatusType.Atk:
                _playerData.Atk += amount; finalValue = _playerData.Atk;
                break;
            case Define.StatusType.HP:
                _playerData.HP += amount*2; finalValue = _playerData.HP;
                break;
            case Define.StatusType.HPRecoveryPerSec:
                _playerData.HPRecoveryPerSec += amount/5; finalValue = _playerData.HPRecoveryPerSec;
                break;
            case Define.StatusType.Def:
                _playerData.Def += amount; finalValue = _playerData.Def;
                break;
        }
        PlayerManager.Instance.Player.ApplyUpgradeStatus(_playerData);
        return finalValue;
    }
}