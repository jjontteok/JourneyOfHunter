using extension;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] TMP_Text _playerNameText;
    [SerializeField] Button _statusButton;
    [SerializeField] Button _inventoryButton;
    [SerializeField] Button _skillInventoryButton;
    [SerializeField] Button _gachaButton;
    [SerializeField] Button _settingButton;
    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] TMP_Text _gemText;
    [SerializeField] Toggle _autoToggle;
    [SerializeField] Toggle _doubleSpeedToggle;
    [SerializeField] GameObject _systemTextPanel;
    // 불/물/빛/암
    [SerializeField] GameObject[] _attributePrefab = new GameObject[4];
    [SerializeField] Image _hpBar;
    [SerializeField] UI_StatusEffect _statusEffect;

    private PlayerController _player;
    private Inventory _playerInventory;

    private int _currentPlayers;

    private string _playerName;

    public string PlayerName
    {
        get { return _playerName; }
        set
        {
            _playerName = value;
            _playerNameText.text = _playerName;
        }
    }

    public UI_StatusEffect StatusEffect
    {
        get { return _statusEffect; }
    }

    public Action<bool> OnAutoChanged;
    public Action<bool> OnDoubleSpeedChanged;

    void Initialize()
    {
        ReleaseEvent();
        _player = PlayerManager.Instance.Player;
        _playerInventory = _player.Inventory;

        MonsterController.OnMonsterDead += GainGoods;
        PlayerManager.Instance.Player.OnHPValueChanged += UpdatePlayerHp;
        EnvironmentManager.Instance.OnTypeChanged += UpdateAttribute;
        EnvironmentManager.Instance.OnTypeChanged?.Invoke(EnvironmentManager.Instance.CurrentType);
        _playerInventory.OnValueChanged += UpdateGoods;

        _statusButton.onClick.AddListener(OnStatusButtonClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
        _skillInventoryButton.onClick.AddListener(OnSkillInventoryButtonClick);
        _gachaButton.onClick.AddListener(OnGachaButtonClick);
        _settingButton.onClick.AddListener(OnSettingButtonClick);

        _silverCoinText.text = _playerInventory.Goods[Define.GoodsType.SilverCoin].ToString();
        _gemText.text = _playerInventory.Goods[Define.GoodsType.Gem].ToString();
        _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
        _doubleSpeedToggle.onValueChanged.AddListener(OnDoubleSpeedToggleClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);

        OnAutoChanged += (flag) => PlayerManager.Instance.IsAuto = flag;
        _player.OnAutoOff += OnAutoToggleOff;

        OnDoubleSpeedChanged += (flag) => TimeManager.Instance.IsDoubleSpeed = flag;
        _player.OnJourneyExpChanged += OnSystemMessage;
        _doubleSpeedToggle.gameObject.SetActive(false);
    }

    private void ReleaseEvent()
    {
        //MonsterController.OnMonsterDead -= GainGoods;
        //_inventoryData.OnValueChanged -= UpdateGoods;
        //_playerVitalCanvas.SetActive(false);
    }

    private void Start()
    {
        Initialize();
    }

    // 변경 필수
    //얘도 몬스터 처치 시 재화를 얼만큼 획득할지 정해야 한당
    void GainGoods()
    {
        PlayerManager.Instance.Player.Inventory.AddGoods(Define.GoodsType.SilverCoin, 100);
    }

    void UpdateGoods(Define.GoodsType type)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                _silverCoinText.text = _playerInventory.Goods[Define.GoodsType.SilverCoin].ToString();
                break;
            case Define.GoodsType.Gem:
                _gemText.text = _playerInventory.Goods[Define.GoodsType.Gem].ToString();
                break;
        }
    }

    void OnStatusButtonClick()
    {
        PopupUIManager.Instance.ActivateStatusPanel();
        AudioManager.Instance.PlayClickSound();
    }

    void OnInventoryButtonClick()
    {
        PopupUIManager.Instance.ActivateInventoryPanel();
        AudioManager.Instance.PlayClickSound();
    }

    void OnSkillInventoryButtonClick()
    {
        PopupUIManager.Instance.ActivateSkillInventoryPanel();
        AudioManager.Instance.PlayClickSound();
    }

    void OnGachaButtonClick()
    {
        PopupUIManager.Instance.ActivateGachaPanel();
        AudioManager.Instance.PlayClickSound();
    }

    void OnSettingButtonClick()
    {
        PopupUIManager.Instance.ActivateSettingPanel();
        AudioManager.Instance.PlayClickSound();
    }

    void OnAutoToggleClick(bool flag)
    {
        OnAutoChanged?.Invoke(flag);
        AudioManager.Instance.PlayClickSound();
    }

    void OnDoubleSpeedToggleClick(bool flag)
    {
        OnDoubleSpeedChanged?.Invoke(flag);
        _doubleSpeedToggle.isOn = flag;
        AudioManager.Instance.PlayClickSound();
    }

    void OnAutoToggleOff()
    {
        _autoToggle.isOn = false;
    }

    void OnSystemMessage(float score)
    {
        string text = $"여정의 증표 {score} 획득";
        SystemTextController[] activeTexts = _systemTextPanel.GetComponentsInChildren<SystemTextController>(false);
        foreach (SystemTextController activeText in activeTexts)
        {
            activeText.UpEffect();
        }

        GameObject systemText = PoolManager.Instance.GetObjectFromPool<SystemTextController>(new Vector3(50, 378, 0), "SystemText");
        systemText.gameObject.transform.SetParent(_systemTextPanel.transform, true);
        systemText.GetComponent<SystemTextController>().SetText(text);
    }

    void UpdatePlayerHp(float currentHP, float maxHP)
    {
        _hpBar.fillAmount = currentHP / maxHP;
    }

    void UpdateAttribute(Define.TimeOfDayType type)
    {
        switch (type)
        {
            // 아침 - 물, 빛
            case Define.TimeOfDayType.Morning:
                _attributePrefab[0].SetActive(false);
                _attributePrefab[1].SetActive(true);
                _attributePrefab[2].SetActive(true);
                _attributePrefab[3].SetActive(false);
                break;
            // 낮 - 불, 빛
            case Define.TimeOfDayType.Noon:
                _attributePrefab[0].SetActive(true);
                _attributePrefab[1].SetActive(false);
                _attributePrefab[2].SetActive(true);
                _attributePrefab[3].SetActive(false);
                break;
            // 저녁 - 어둠, 불
            case Define.TimeOfDayType.Evening:
                _attributePrefab[0].SetActive(true);
                _attributePrefab[1].SetActive(false);
                _attributePrefab[2].SetActive(false);
                _attributePrefab[3].SetActive(true);
                break;
            // 밤 - 어둠, 물
            case Define.TimeOfDayType.Night:
                _attributePrefab[0].SetActive(false);
                _attributePrefab[1].SetActive(true);
                _attributePrefab[2].SetActive(false);
                _attributePrefab[3].SetActive(true);
                break;
            default:
                break;
        }
    }
}
