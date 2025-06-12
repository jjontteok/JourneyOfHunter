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
    [SerializeField] Button _gainedGoodsButton;
    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] TMP_Text _gemText;
    [SerializeField] Toggle _autoToggle;
    [SerializeField] PlayerInventoryData _inventoryData;

    private List<UI_PlayerVital> _playerVitalList; 
    private GameObject _playerVitalCanvas;

    private int _currentPlayers;


    private void Awake()
    {
        _playerVitalList = new List<UI_PlayerVital>();
    }
    public Action<bool> OnAutoChanged;


    void Initialize()
    {
        MonsterController.OnMonsterDead += GainGoods;
        _inventoryData.OnValueChanged += UpdateGoods;

        _statusButton.onClick.AddListener(OnStatusButtonClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
        _gainedGoodsButton.onClick.AddListener(OnGainedGoodsButtonClick);
        _silverCoinText.text = _inventoryData.silverCoin.ToString();
        _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        OnAutoChanged += (flag) => player.IsAuto = flag;
        player.OnAutoOff += OnAutoToggleOff;
    }

    private void OnDisable()
    {
        MonsterController.OnMonsterDead -= GainGoods;
        _inventoryData.OnValueChanged -= UpdateGoods;
        _playerVitalCanvas.SetActive(false);
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

    //얘도 몬스터 처치 시 재화를 얼만큼 획득할지 정해야 한당
    void GainGoods()
    {
        Define.GoodsType type;
        float amount;
        //확률을 이렇게 하는 게 맞?나
        if (Util.Probability(0.3f))
        {
            type = Define.GoodsType.SilverCoin;
            amount = 100;
        }
        else if (Util.Probability(0.6f))
        {
            type = Define.GoodsType.Exp;
            amount = 10;
        }
        else
        {
            type = Define.GoodsType.EnhancementStone;
            amount = 5;
        }
        _inventoryData.ModifyGoods(type, amount);

        PopupUIManager.Instance.UpdateGainedRecord(type, amount);
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

    void OnGainedGoodsButtonClick()
    {
        PopupUIManager.Instance.ActivateGainedRecordPanel(Define.GoodsType.None, 0);
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
