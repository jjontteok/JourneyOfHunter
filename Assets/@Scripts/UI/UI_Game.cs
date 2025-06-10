using extension;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] Button _statusButton;
    [SerializeField] Button _inventoryButton;
    [SerializeField] Button _gainedGoodsButton;
    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] TMP_Text _gemText;
    [SerializeField] Toggle _autoToggle;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] PlayerInventoryData _inventoryData;

    private List<UI_PlayerVital> _playerVitalList; 
    private GameObject _playerVitalCanvas;

    private int _minute;
    private int _second;

    private int _currentPlayers;

    private void Awake()
    {
        _playerVitalList = new List<UI_PlayerVital>();
    }
    public Action<bool> OnAutoChanged;


    void Initialize()
    {
        TimeManager.Instance.OnTimeChanged += UpdateTimeText;
        _inventoryData.OnValueChanged += UpdateGoods;


        _statusButton.onClick.AddListener(OnStatusButtonClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
        _gainedGoodsButton.onClick.AddListener(OnReceivedGoodsButtonClick);
        _silverCoinText.text = _inventoryData.silverCoin.ToString();
        _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
        // _inventoryButton.onClick.AddListener(OnInventoryButtonClick);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        //OnAutoChanged += player.SetAuto;
        OnAutoChanged += (flag) => player.IsAuto = flag;
        player.OnAutoOff += OnAutoToggleOff;
    }

    //private void OnEnable()
    //{
    //    TimeManager.Instance.OnTimeChanged += SetTimeText;
    //    _statusButton.onClick.AddListener(OnStatusButtonClick);
    //    _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
    //   // _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
    //}

    private void OnDisable()
    {
        TimeManager.Instance.OnTimeChanged -= UpdateTimeText;
        _inventoryData.OnValueChanged -= UpdateGoods;
    }

    private void Start()
    {
        Initialize();
        _playerVitalCanvas = Instantiate(ObjectManager.Instance.PlayerVitalCanvas);
        Canvas canvas = _playerVitalCanvas.GetOrAddComponent<Canvas>();
        
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

    void UpdateTimeText(float time)
    {
        _minute = (int)time / 60;
        _second = (int)time % 60;
        _timeText.text = _minute.ToString("00") + " : " + _second.ToString("00");
    }

    void UpdateGoods(Define.GoodsType type)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                _silverCoinText.text = _inventoryData.silverCoin.ToString();
                break;
            case Define.GoodsType.Exp:
                break;
            case Define.GoodsType.EnhancementStone:
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

    void OnReceivedGoodsButtonClick()
    {
        PopupUIManager.Instance.ActivateGainedRecordPanel();
    }
    void OnAutoToggleClick(bool flag)
    {
        Debug.Log($"Auto: {flag}");
        OnAutoChanged?.Invoke(flag);
    }

    void OnAutoToggleOff()
    {
        _autoToggle.isOn = false;
    }
}
