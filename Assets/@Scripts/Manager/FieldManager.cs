using System;
using System.Collections.Generic;
using UnityEngine;


public class RewardSystem
{
    Dictionary<Define.RewardType, int> _rewardList;

    public RewardSystem()
    {
        _rewardList = new();
    }

    public void SetReward(Define.RewardType rewardType, int rewardAmount)
    {
        _rewardList[rewardType] = rewardAmount;
    }

    public void GainReward(Vector3 pos)
    {
        foreach (var reward in _rewardList)
        {
            string rewardName = "";
            if (reward.Key == Define.RewardType.JourneyExp)
            {
                rewardName = "여정의 증표";
                PlayerManager.Instance.Player.GainJourneyExp(reward.Value);
            }
            else if (reward.Key == Define.RewardType.SilverCoin)
            {
                rewardName = "은화";
                PlayerManager.Instance.Player.Inventory.AddGoods(Define.GoodsType.SilverCoin, reward.Value);
            }
            else if (reward.Key == Define.RewardType.Gem)
            {
                rewardName = "젬";
                PlayerManager.Instance.Player.Inventory.AddGoods(Define.GoodsType.Gem, reward.Value);
            }
            TextManager.Instance.ActivateRewardText(pos, rewardName, reward.Value);
        }
    }
}

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    public event Action<int> OnStageChanged;
    public event Action<Define.JourneyType, Define.ItemValue> OnJourneyEvent;
    public event Action OnFailedDungeonClear;
    public event Action<int> OnUpgradeMonsterStatus;
    public event Action OnMerchantAppeared;

    StageController _stageController;
    SpawnController _spawnController;
    DungeonController _dungeonController;
    RewardSystem _rewardSystem;
    [SerializeField]
    Define.JourneyType _currentType;

    int _stageCount;
    int _failedCount = 0;
    bool _isClear = false;

    public StageController StageController
    {
        get { return _stageController; }
    }

    public DungeonController DungeonController
    {
        get { return _dungeonController; }
    }

    public int StageCount
    {
        get { return _stageController.StageCount; }
        set { _stageController.StageCount = value; }
    }

    public RewardSystem RewardSystem
    {
        get { return _rewardSystem; }
    }

    public int FailedCount
    {
        get { return _failedCount; }
    }

    public Define.JourneyType CurrentEventType
    {
        get { return _currentType; }
    }

    public bool IsClear
    {
        get
        {
            return _isClear;
        }
        set
        {
            _isClear = value;
        }
    }

    protected override void Initialize()
    {
        _dungeonController = new GameObject("DungeonController").AddComponent<DungeonController>();
        _stageController = new GameObject("StageController").AddComponent<StageController>();
        _spawnController = new GameObject("SpawnController").AddComponent<SpawnController>();

        _rewardSystem = new RewardSystem();
        _stageCount = StageCount;
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        PlayerManager.Instance.Player.OnPlayerCrossed += OnPlayerCross;
        PlayerManager.Instance.Player.OnPlayerDead += FailedDungeonClear;
        _dungeonController.OnDungeonClear += SuccessDungeonClear;
    }
    #endregion

    #region FieldUpdate
    //플레이어가 구역을 지나면 호출될 함수
    void OnPlayerCross()
    {
        _stageCount = ++StageCount;
        OnStageChanged?.Invoke(_stageCount);
        IsClear = false;

        if (_stageCount % 10 == 0)
        {
            OnUpgradeMonsterStatus?.Invoke(_stageCount / 10);
        }

        Define.ItemValue rank = SetRank();
        //5의 배수마다 던전 두두둥장
        if (_stageCount % 5 == 0)
        {
            _rewardSystem.SetReward(Define.RewardType.JourneyExp, 10 * _stageCount);
            _currentType = Define.JourneyType.Dungeon;
            rank = Define.ItemValue.Common;
        }
        else
        {
            int rnd = UnityEngine.Random.Range(0, 100);
            if (rnd < 90)
            {
                //80% 확률로 기타 오브젝트 등장
                if (rnd < 80)
                //if (rnd < 50)
                {
                    _currentType = Define.JourneyType.OtherObject;
                    _rewardSystem.SetReward(Define.RewardType.JourneyExp, 25 * _stageCount * (int)(rank + 1));
                }
                //10% 확률로 보물상자 등장
                else
                {
                    _currentType = Define.JourneyType.TreasureBox;
                    _rewardSystem.SetReward(Define.RewardType.JourneyExp, 50 * _stageCount * (int)(rank + 1));
                    _rewardSystem.SetReward(Define.RewardType.Gem, 10 * _stageCount * (int)(rank + 1));
                    _rewardSystem.SetReward(Define.RewardType.SilverCoin, 100 * _stageCount * (int)(rank + 1));
                }
            }
            //10%의 확률로 상인 등장
            else
            {
                _currentType = Define.JourneyType.Merchant;
                rank = Define.ItemValue.Common;
                OnMerchantAppeared?.Invoke();
            }
        }
        OnJourneyEvent?.Invoke(_currentType, rank);
        PlayerManager.Instance.IsAutoMoving = false;
        PlayerManager.Instance.Player.ReleaseTarget();
    }
    #endregion

    // * 보물상자의 등급을 정하는 함수
    Define.ItemValue SetRank()
    {
        int treasureRank = UnityEngine.Random.Range(1, 101);
        if (treasureRank < 50)
            return Define.ItemValue.Common;         //50% : Common
        else if (treasureRank < 70)
            return Define.ItemValue.Uncommon;       //20% : Uncommon
        else if (treasureRank < 85)
            return Define.ItemValue.Rare;           //15% : Rare
        else if (treasureRank < 95)
            return Define.ItemValue.Epic;           //10% : Epic
        else
            return Define.ItemValue.Legendary;      //5% : Legendary
    }

    void SuccessDungeonClear()
    {
        DungeonClear(true);
        _rewardSystem.GainReward(PlayerManager.Instance.Player.transform.position + Vector3.up * 3);
        PlayerManager.Instance.Player.RemovePlayerBuff(_failedCount);
        _failedCount = 0;
    }

    // * 던전을 깨지 못했을 떄 실행되는 함수
    public void FailedDungeonClear()
    {
        DungeonClear(false);
        StageCount -= 5;
        _stageCount = StageCount;
        OnFailedDungeonClear?.Invoke();
        if (_failedCount < 3)
        {
            _failedCount++;
            PlayerManager.Instance.Player.SetPlayerBuff();
        }
    }

    void DungeonClear(bool isClear)
    {
        PopupUIManager.Instance.ActivateDungeonClearText(isClear);
        //플레이어 체력 초기화
        PlayerManager.Instance.Player.HP = PlayerManager.Instance.Player.PlayerData.HP;
        //타이머 초기화
        TimeManager.Instance.StopNamedMonsterTimer();
    }

    public void Deactivate()
    {

    }
}
