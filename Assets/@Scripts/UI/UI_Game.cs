using extension;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] TMP_Text _playerLevelText;
    [SerializeField] Button _statusButton;
    [SerializeField] Button _inventoryButton;
    [SerializeField] Button _createDungeonPortalButton;
    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] TMP_Text _gemText;
    [SerializeField] Toggle _autoToggle;
    [SerializeField] Toggle _doubleSpeedToggle;
    [SerializeField] PlayerInventoryData _inventoryData;
    [SerializeField] GameObject _systemTextPanel;

    private List<UI_PlayerVital> _playerVitalList; 
    private GameObject _playerVitalCanvas;
    private PlayerController _player;

    private int _currentPlayers;

    private void Awake()
    {
        _playerVitalList = new List<UI_PlayerVital>();
    }
    public Action<bool> OnAutoChanged;
    public Action<bool> OnDoubleSpeedChanged;

    void Initialize()
    {
        ReleaseEvent();

        MonsterController.OnMonsterDead += GainGoods;
        //_inventoryData.OnValueChanged += UpdateGoods;

        _statusButton.onClick.AddListener(OnStatusButtonClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
        _createDungeonPortalButton.onClick.AddListener(OnCreateDungeonButtonClick);
        _silverCoinText.text = _inventoryData.SilverCoin.ToString();
        _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
        _doubleSpeedToggle.onValueChanged.AddListener(OnDoubleSpeedToggleClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);

        _player = PlayerManager.Instance.Player;
        OnAutoChanged += (flag) => PlayerManager.Instance.IsAuto = flag;
        _player.OnAutoOff += OnAutoToggleOff;
        _player.OnAutoDungeonChallenge += () =>
        {
            if (PlayerManager.Instance.IsAuto)
            {
                OnCreateDungeonButtonClick();
            }
        };

        OnDoubleSpeedChanged += (flag) => TimeManager.Instance.IsDoubleSpeed = flag;
        _player.OnJourneyExpChanged += OnSystemMessage;
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
        _playerVitalCanvas = Instantiate(ObjectManager.Instance.PlayerVitalCanvas);
        Canvas canvas = _playerVitalCanvas.GetOrAddComponent<Canvas>();
        
        //다 모듈화해주세용
        //플레이어 리스트를 받아와서
        GameObject[] players = GameObject.FindGameObjectsWithTag(Define.PlayerTag);
        _currentPlayers = players.Length;

        //플레이어 수대로 playerVital을 만들어서 리스트에 넣기
        for(int i =0; i< _currentPlayers; i++)
        {
            GameObject playerVital = Instantiate(ObjectManager.Instance.PlayerVitalResource);
            playerVital.transform.SetParent(_playerVitalCanvas.transform);
            UI_PlayerVital uiPlayerVital = playerVital.GetOrAddComponent<UI_PlayerVital>();
            uiPlayerVital.Initialize(players[i].transform);
            _playerVitalList.Add(uiPlayerVital);
        }

    }

    // 변경 필수
    //얘도 몬스터 처치 시 재화를 얼만큼 획득할지 정해야 한당
    void GainGoods()
    {
        Define.GoodsType type;
        float amount;
        float r = UnityEngine.Random.Range(0, 1);
        if (r < 0.3f)
        {
            type = Define.GoodsType.SilverCoin;
            amount = 100;
        }
    }

    void UpdateGoods(Define.GoodsType type)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                _silverCoinText.text = _inventoryData.SilverCoin.ToString();
                break;
            case Define.GoodsType.Gem:
                break;
        }
    }

    void OnStatusButtonClick()
    {
        PopupUIManager.Instance.ActivateStatusPanel();
    }

    void OnInventoryButtonClick()
    {
        PopupUIManager.Instance.ActivateInventoryPanel();
    }

    void OnCreateDungeonButtonClick()
    {
        FieldManager.Instance.DungeonController.CreateDungeon();
        FieldManager.Instance.DungeonController.OnDungeonExit -= ActivateDungeonPortalButton;
        FieldManager.Instance.DungeonController.OnDungeonExit += ActivateDungeonPortalButton;
        _createDungeonPortalButton.gameObject.SetActive(false);
    }

    void OnAutoToggleClick(bool flag)
    {
        //Debug.Log($"Auto: {flag}");
        OnAutoChanged?.Invoke(flag);
    }

    void OnDoubleSpeedToggleClick(bool flag)
    {
        OnDoubleSpeedChanged?.Invoke(flag);
        _doubleSpeedToggle.isOn = flag;
    }

    void OnAutoToggleOff()
    {
        _autoToggle.isOn = false;
    }

    void ActivateDungeonPortalButton()
    {
        if(!PlayerManager.Instance.IsAuto)
        {
            _createDungeonPortalButton.gameObject.SetActive(true);
        }
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
}
