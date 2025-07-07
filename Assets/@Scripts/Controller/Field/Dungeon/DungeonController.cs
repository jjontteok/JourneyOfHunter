using System;
using UnityEngine;
using System.Collections.Generic;

public class DungeonController : MonoBehaviour
{
    public Vector3 DungeonOffSet;

    #region DungeonObject
    private GameObject _dungeonWallFront;
    private GameObject _dungeonWallBack;
    private GameObject _dungeonEnterPortal;
    private GameObject _dungeonExitPortal;

    private DungeonPortalController _portalEnterController;
    private DungeonPortalController _portalExitController;
    #endregion

    #region StageInfo
    private int _deathMonsterCount = 0;
    private bool _isOnSpawnableInvoked = false;
    private bool _isDungeonExist = false;
    #endregion

    #region Variables
    private bool _isChallenge = false;
    #endregion

    #region Property
    public GameObject DungeonWallFront
    {
        get { return _dungeonWallFront; }
    }
    public GameObject DungeonWallBack
    {
        get { return _dungeonWallBack; }
    }
    public GameObject DungeonEnterPortal
    {
        get { return _dungeonEnterPortal; }
    }
    public GameObject DungeonExitPortal
    {
        get { return _dungeonExitPortal; }
    }
    //public GameObject MonsterGateList
    //{
    //    get { return _monsterGate; }
    //}
    public bool IsChallenge
    {
        get { return _isChallenge; }
        set
        {
            if (value == true)
                OnSpawnNamedMonster?.Invoke();
            _isChallenge = value;
        }
    }

    public bool IsDungeonExist
    {
        get { return _isDungeonExist; }
        set { _isDungeonExist = value; }
    }

    public int DeathMonsterCount
    {
        get { return _deathMonsterCount; }
        set
        {
            _deathMonsterCount = value;
            OnNormalMonsterDead?.Invoke(_deathMonsterCount, 20);
        }
    }
    #endregion

    #region Action
    public Action OnDungeonEnter;
    public Action OnDungeonClear;
    public Action OnDungeonFail;
    public Action OnDungeonExit;

    public Action<float, float> OnNormalMonsterDead;
    public Action OnSpawnableNamedMonster;
    public Action OnSpawnNamedMonster;
    #endregion

    #region Initialize

    private void Awake()
    {
        Initialize();
        Deactivate();
    }
    void Initialize()
    {
        _dungeonWallFront = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonWallBack = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonEnterPortal = Instantiate(ObjectManager.Instance.DungeonPortalResource);
        _dungeonEnterPortal.name = "EnterPortal";
        _dungeonExitPortal = Instantiate(ObjectManager.Instance.DungeonPortalResource);
        _dungeonExitPortal.name = "ExitPortal";

        _portalEnterController = _dungeonEnterPortal.GetComponent<DungeonPortalController>();
        _portalExitController = _dungeonExitPortal.GetComponent<DungeonPortalController>();

    }
    #endregion

    void OnEnable()
    {
        Subscribe();
    }

    void Subscribe()
    {
        NormalMonsterController.s_OnNormalMonsterDie += CountMonsterDeath;
        NamedMonsterController.s_OnNamedMonsterDie += ClearDungeon;
        FieldManager.Instance.OnJourneyEvent += CreateDungeon;
        FieldManager.Instance.OnFailedDungeonClear += ExitDungeon;

        _portalEnterController.OnPotalEnter += EnterDungeon;
        _portalEnterController.OnPotalClose += SetWallUp;

        _portalExitController.OnPotalEnter += ExitDungeon;
        //_portalExitController.OnPotalClose += ExitDungeon;

        //OnSpawnNamedMonster += SetNormalMonsterOff;

        OnDungeonEnter += () => IsDungeonExist = true;
        OnDungeonExit += () => IsDungeonExist = false;
    }

    public void Deactivate()
    {
        DungeonWallFront.SetActive(false);
        DungeonWallBack.SetActive(false);
        DungeonEnterPortal.SetActive(false);
        DungeonExitPortal.SetActive(false);
    }

    // * 스테이지 정보 로드
    private void SetStageInfo()
    {

    }

    // * 던전 생성 메서드
    public void CreateDungeon(Define.JourneyType type, Define.ItemValue value = default)
    {
        if (type == Define.JourneyType.Dungeon)
        {
            DungeonOffSet = new Vector3(0, 0, Define.DungeonEnterPortalSpot.z);

            //SetStageInfo();

            DungeonEnterPortal.SetActive(true);
            DungeonEnterPortal.transform.position = Define.DungeonEnterPortalSpot;
            DungeonWallBack.SetActive(true);
            DungeonWallBack.transform.position = Define.DungeonExitSpot;
            DungeonWallFront.transform.position = Define.DungeonEnterSpot;
        }
    }

    // * 던전 폐쇄 메서드
    private void DestroyDungeon()
    {

    }

    // * 포탈 입장 액션 구독 메서드
    //- 던전 입장 기능
    private void EnterDungeon()
    {
        _isOnSpawnableInvoked = false;
        DeathMonsterCount = 0;
        SetWallDown();
        //PopupUIManager에서 stageInfo 활성화, adventureInfo 비활성화
        OnDungeonEnter?.Invoke();
    }

    // * 네임드 몬스터 사망 액션 구독 메서드
    //- 던전 클리어 후처리
    private void ClearDungeon()
    {
        OnDungeonClear?.Invoke(); //네임드 인포 비활성화, 보상획득
        SetClearPortalOn();
    }

    // * 던전 나가거나, 던전 클리어 실패 시 실행되는 메서드
    private void ExitDungeon()
    {
        //PopupUIManager에서 stageInfo 비활성화, adventureInfo 활성화
        //네임드 몬스터 비활성, 일반 몬스터 비활성
        OnDungeonExit?.Invoke();
        SetWallDown();
    }

    // * 던전 오브젝트 관리 메서드
    private void SetClearPortalOn()
    {
        _dungeonExitPortal.SetActive(true);
        _dungeonExitPortal.transform.position = Define.DungeonExitPortalSpot;
    }
    private void SetWallDown()
    {
        _dungeonWallFront.SetActive(false);
        _dungeonWallBack.SetActive(false);
    }
    private void SetWallUp()
    {
        _dungeonWallFront.SetActive(true);
        _dungeonWallBack.SetActive(true);
    }

    // * 던전 몬스터 관리 메서드
    private void CountMonsterDeath()
    {
        DeathMonsterCount++;

        if (_deathMonsterCount >= 20 && !_isOnSpawnableInvoked)
        {
            _isOnSpawnableInvoked = true;
            OnSpawnableNamedMonster?.Invoke(); //도전 버튼 활성화해야함
        }
    }
}