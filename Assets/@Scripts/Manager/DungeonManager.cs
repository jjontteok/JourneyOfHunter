using System;
using UnityEngine;
using extension;
using System.Collections;

public class DungeonManager : Singleton<DungeonManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _dungeonWallFront;
    private GameObject _dungeonWallBack;
    private GameObject _dungeonPortal;

    private DungeonPortalController _portalController;

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

    #region Action
    public Action OnDungeonEnter;
    public Action OnDungeonClear;
    public Action OnDungeonFail;
    public Action OnDungeonExit;

    public Action OnSpawnNamedMonster;
    #endregion

    #region Initialize
    protected override void Initialize()
    {
        _dungeonWallFront = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonWallBack = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonPortal = Instantiate(ObjectManager.Instance.DungeonPortalResource);

        _portalController = _dungeonPortal.GetComponent<DungeonPortalController>();
    }
    #endregion

    #region IEventSubscriber
    public void Subscribe()
    {
        //PopupUIManager.Instance.OnButtonDungeonEnterClick += EnterDungeon;
        _portalController.OnPotalEnter += EnterDungeon;
        _portalController.OnPotalClose += SetWallUp;
    }
    #endregion

    #region IDeactivateObject
    public void Deactivate()
    {
        DungeonWallFront.SetActive(false);
        DungeonWallBack.SetActive(false);
        DungeonPortal.SetActive(false);

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
        OnDungeonEnter?.Invoke();
    }


    private void SetPortalOn()
    {
        _dungeonPortal.SetActive(true);
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
}
