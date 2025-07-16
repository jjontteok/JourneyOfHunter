using extension;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, IEventSubscriber
{
    private PlayerController _player;
    private SkillSystem _skillSystem;
    //private InventoryChangeQueue _inventoryChangeQueue;

    private bool _isGameStart = false;
    private bool _isAuto = false;
    private bool _isAutoMoving = false;

    readonly Vector3 _originPos = new Vector3(0f, 0.1f, 0.5f);

    public bool IsCutSceneOn = false;

    #region Properties

    public SkillSystem SkillSystem { get { return _skillSystem; } }

    public bool IsGameStart
    {
        get { return _isGameStart; }
        set { _isGameStart = value; }
    }

    public bool IsAuto
    {
        get { return _isAuto; }
        set
        {
            _isAuto = value;
            // 자동 모드 꺼지면 타겟도 초기화
            // AutoChallenge 중이면 NotChallenge로 변화
            if (!value)
            {
                _player.Target = null;
                //_isAutoMoving = false;
                if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.AutoChallenge)
                {
                    FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.NotChallenge;
                }
            }
            else
            {
                if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.NotChallenge)
                {
                    // 던전 생성 버튼이 활성화되어있는데 자동 모드 켜질때
                    if (!FieldManager.Instance.DungeonController.IsDungeonExist)
                    {
                        _player.OnAutoDungeonChallenge?.Invoke();
                    }
                    // 던전 진행 중이고, 아직 게이지 다 안 찬 NotChallenge상태에서 자동 켜질 때
                    else if (FieldManager.Instance.DungeonController.IsDungeonExist)
                    {
                        FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.AutoChallenge;
                    }
                }
                else if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.Challenge)
                {
                    FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.Challenging;
                    FieldManager.Instance.StageController.IsSpawnNamedMonster = true;
                    //FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.AutoChallenge;
                }
            }
            Debug.Log($"Current AutoMoving: {_isAutoMoving}, Current IsClear: {FieldManager.Instance.IsClear}");
        }
    }

    // 하나의 필드 내에서 이벤트가 끝나고 다음 필드로 향할 때
    // 던전 => 포탈을 향해 가야할 때(target==portal), 몬스터랑 포탈 다 없는 잠깐의 순간(target==null)
    // 오브젝트 => 상호작용을 통해 보상을 얻고 난 후
    public bool IsAutoMoving
    {
        get { return _isAutoMoving; }
        set
        {
            if (_isAutoMoving != value)
            {
                _isAutoMoving = value;
            }
        }
    }

    public bool IsDead { get; set; }

    public PlayerController Player
    {
        get { return _player; }
    }

    //public InventoryChangeQueue InventoryChangeQueue
    //{
    //    get { return _inventoryChangeQueue; }
    //}
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        _player = Instantiate(ObjectManager.Instance.PlayerResource, _originPos, Quaternion.identity).GetComponent<PlayerController>();
        _skillSystem = _player.gameObject.GetOrAddComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();
        //_inventoryChangeQueue = new InventoryChangeQueue();
    }

    public void Subscribe()
    {
        UIManager.Instance.UI_Main.OnStartButtonClicked += () => _isGameStart = true;
        //_player.Inventory.OnItemAdd += _inventoryChangeQueue.PushTask;
        //_player.Inventory.OnItemRemove += InventoryChangeQueue.PushTask;
    }
}


//// 변경사항 구조체
//// 아이템 변경 사항 내용
//// 1. 아이템 생성 및 삭제
//public class PendingChange
//{
//    public Define.PendingTaskType TaskType;
//    public int Value;

//    public PendingChange(Define.PendingTaskType pendingTaskType, int value)
//    {
//        TaskType = pendingTaskType;
//        Value = value;
//    }
//}

//// 대기중인 변경사항 관리 클래스
//// 작업 별 큐에 변경사항 작업 내용 저장
//// 해당 인벤토리 탭 오픈 시에 작업 내용 pop 및 적용
//// 우선 아이템 타입별로 작업 내용이 존재하는 지를 확인할 수 있어야 함 -> 이걸 체크해서 불필요한 과정을 줄이자. 그냥 queue의 사이즈를 보고 판단하면 될듯?
//public class InventoryChangeQueue
//{
//    private Dictionary<Define.ItemType, Queue<PendingChange>> _pendingChangeList;
//    private Dictionary<Define.ItemType, List<ItemData>> _cachedInventory;

//    public InventoryChangeQueue()
//    {
//        _pendingChangeList = new Dictionary<Define.ItemType, Queue<PendingChange>>();

//        foreach (Define.ItemType itemType in Enum.GetValues(typeof(Define.ItemType)))
//        {
//            _pendingChangeList.Add(itemType, new Queue<PendingChange>());
//        }
//    }

//    public bool IsExistTask(Define.ItemType itemType)
//    {
//        if (_pendingChangeList[itemType].Count > 0)
//            return true;
//        return false;
//    }

//    public void PushTask(Define.ItemType itemType, Define.PendingTaskType taskType, int value)
//    {
//        PendingChange pendingChange = new PendingChange(taskType, value);
//        _pendingChangeList[itemType].Enqueue(pendingChange);
//    }

//    public PendingChange PopTask(Define.ItemType itemType)
//    {
//        if (!IsExistTask(itemType))
//            return null;
//        return _pendingChangeList[itemType].Dequeue();
//    }
//}
