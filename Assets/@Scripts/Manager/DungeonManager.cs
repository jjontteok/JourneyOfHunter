using System;
using UnityEngine;
using extension;
using System.Collections;
using System.Collections.Generic;

// 스테이지 정보 구조체...
// 스테이지 정보를 불러온다 어떻게? 
public struct StageInfo
{
    public int StageCount;
    public int ClearCount;

    public string NormalMonsterName;
    public string NamedMonsterName;
}

public class DungeonManager : Singleton<DungeonManager>, IEventSubscriber, IDeactivateObject
{
    #region DungeonObject
    private GameObject _dungeonWallFront;
    private GameObject _dungeonWallBack;
    private GameObject _dungeonPortal;
    private GameObject _monsterGate;

    private DungeonPortalController _portalController;
    #endregion

    #region StageInfo
    private Dictionary<string, StageInfo> _stages;
    private StageInfo _currentStage;    

    private static int s_deathMonsterCount = 0;
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
    public GameObject DungeonPortal
    { 
        get { return _dungeonPortal; } 
    }
    public GameObject MonsterGateList
    {
        get { return _monsterGate; }
    }
    #endregion

    #region Action
    public Action OnDungeonEnter;
    public Action OnDungeonClear;
    public Action OnDungeonFail;
    public Action OnDungeonExit;

    public Action OnSpawnableNamedMonster;
    public Action OnSpawnNamedMonster;
    #endregion

    #region Initialize
    protected override void Initialize()
    {
        _dungeonWallFront = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonWallBack = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonPortal = Instantiate(ObjectManager.Instance.DungeonPortalResource);

        _monsterGate = Instantiate(ObjectManager.Instance.MonsterGateResource, Define.SpawnSpot5 + Vector3.up *11 , Quaternion.Euler(-135,0,0));

        _portalController = _dungeonPortal.GetComponent<DungeonPortalController>();

        _stages = new Dictionary<string, StageInfo>();
    }
    #endregion

    #region IEventSubscriber
    public void Subscribe()
    {
        //PopupUIManager.Instance.OnButtonDungeonEnterClick += EnterDungeon;
        _portalController.OnPotalEnter += EnterDungeon;
        _portalController.OnPotalClose += SetWallUp;

        NormalMonsterController.s_OnNormalMonsterDie += CountMonsterDeath;
        NamedMonsterController.s_OnNamedMonsterDie += SetClearPortalOn;
    }
    #endregion

    #region IDeactivateObject
    public void Deactivate()
    {
        DungeonWallFront.SetActive(false);
        DungeonWallBack.SetActive(false);
        DungeonPortal.SetActive(false);
        _monsterGate.SetActive(false);

        SetDungeon();
    }
    #endregion

    // * 던전 생성 메서드
    private void SetDungeon()
    {
        DungeonWallFront.SetActive(true);
        DungeonWallFront.transform.position = Define.DungeonEnterSpot;
        DungeonWallBack.SetActive(true);
        DungeonWallBack.transform.position = Define.DungeonExitSpot;
        DungeonPortal.SetActive(true);
        DungeonPortal.transform.position = Define.DungeonEnterPortalSpot;
    }

    // * 포탈 입장 액션 구독 메서드
    //- 던전 입장 기능
    private void EnterDungeon()
    {
        SetWallDown();
        SetMonsterGateOn();
        OnDungeonEnter?.Invoke();
    }

    // * 던전 오브젝트 관리 메서드
    private void SetClearPortalOn()
    {
        _dungeonPortal.SetActive(true);
        _dungeonPortal.transform.position = Define.DungeonExitPortalSpot;
    }
    private void SetWallDown()
    {
        _dungeonWallFront.SetActive(false);
        _dungeonWallBack.SetActive(false);
    }
    private void SetWallUp()
    {
        _dungeonWallBack.SetActive(true);
    }
    private void SetMonsterGateOn()
    {
        _monsterGate.SetActive(true);
    }

    // * 던전 몬스터 관리 메서드
    private void CountMonsterDeath()
    {
        s_deathMonsterCount++;
        if(s_deathMonsterCount >= _currentStage.ClearCount)
        {
            OnSpawnableNamedMonster?.Invoke();
        }
    }
}
