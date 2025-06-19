using System;
using UnityEngine;
using UnityEngine.UI;
using extension;

// * 팝업 UI 매니저 스크립트
//- 특정 이벤트 발생 시 팝업 UI 생성
public class PopupUIManager : Singleton<PopupUIManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _canvasPopupUI;
    private GameObject _popupPanel;
    private GameObject _popupStageInfo;
    private GameObject _popupNamedMonsterInfo;
    private GameObject _panelStatus;
    private GameObject _panelInventory;
    private GameObject _panelSkillInventory;
    private GameObject _panelGainedRecord;

    private GameObject _activePopup;
    public GameObject PanelGainedRecord
    {
        get { return _panelGainedRecord; }
    }

    #region Initialize
    protected override void Initialize()
    {
        _canvasPopupUI = Instantiate(ObjectManager.Instance.PopupCanvas);
        _popupPanel = Instantiate(ObjectManager.Instance.PopupPanel, _canvasPopupUI.transform);
        _popupStageInfo = Instantiate(ObjectManager.Instance.PopupStageInfo, _canvasPopupUI.transform);
        _popupNamedMonsterInfo = Instantiate(ObjectManager.Instance.PopupNamedMonsterInfo, _canvasPopupUI.transform);
        _panelStatus = Instantiate(ObjectManager.Instance.PopupStatusPanel, _canvasPopupUI.transform);
        _panelInventory = Instantiate(ObjectManager.Instance.PopupInventoryPanel, _canvasPopupUI.transform);
        _panelSkillInventory = Instantiate(ObjectManager.Instance.PopupSkillInventory, _canvasPopupUI.transform);
        _panelGainedRecord = Instantiate(ObjectManager.Instance.PopupGainedRecordPanel, _canvasPopupUI.transform);
    }
    #endregion


    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnDungeonEnter += ActivateStageInfoPanel;
        DungeonManager.Instance.OnDungeonExit += DeactivateStageInfoPanel;
        DungeonManager.Instance.OnSpawnNamedMonster += DeactivateStageInfoPanel;
        //CameraManager.Instance.OnCutSceneEnded += ActivateNamedMonsterInfoPanel;
        //DungeonManager.Instance.OnSpawnNamedMonster += ActivateNamedMonsterInfoPanel;
        DungeonManager.Instance.OnDungeonClear += DeactivateNamedMonsterInfo;

        TimeManager.Instance.OnGainedRecordTimeChanged += UpdateGainedRecordTime;
        _popupPanel.GetComponent<PopupUI_Panel>().OnPopupPanelClicked += DeactivatePopup;
        _panelStatus.GetComponent<PopupUI_Status>().OnExitButtonClicked += DeactivatePopup;
        _panelInventory.GetComponent<PopupUI_Inventory>().OnExitButtonClicked += DeactivatePopup;
        _panelSkillInventory.GetComponent<PopupUI_SkillInventory>().OnExitButtonClicked += DeactivatePopup;
        _panelGainedRecord.GetComponent<PopupUI_GainRecord>().OnExitButtonClicked += DeactivatePopup;
    }
    #endregion

    //void OnDisable()
    //{
    //    //임시 방편
    //    TimeManager.Instance.OnGainedRecordTimeChanged -= UpdateGainedRecordTime;
    //    _popupPanel.GetComponent<PopupUI_Panel>().OnPopupPanelClicked -= DeactivatePopup;
    //    _panelStatus.GetComponent<PopupUI_Status>().OnExitButtonClicked -= DeactivatePopup;
    //    _panelInventory.GetComponent<PopupUI_Inventory>().OnExitButtonClicked -= DeactivatePopup;
    //    _panelSkillInventory.GetComponent<PopupUI_SkillInventory>().OnExitButtonClicked -= DeactivatePopup;
    //    _panelGainedRecord.GetComponent<PopupUI_GainRecord>().OnExitButtonClicked -= DeactivatePopup;
    //}

    #region IDeactivate
    public void Deactivate()
    {
        _popupPanel.SetActive(false);
        _popupStageInfo.SetActive(false);
        _popupNamedMonsterInfo.SetActive(false);
        _panelStatus.SetActive(false);
        _panelInventory.SetActive(false);
        _panelSkillInventory.SetActive(false);
        _panelGainedRecord.SetActive(false);
    }
    #endregion

    #region Activate UI
    void ActivatePopupPanel()
    {
        _popupPanel.SetActive(true);
    }

    public void ActivateStatusPanel()
    {
        ActivatePopupPanel();
        _activePopup = _panelStatus;
        _panelStatus.SetActive(true);
    }

    public void ActivateInventoryPanel()
    {
        ActivatePopupPanel();
        _activePopup = _panelInventory;
        _panelInventory.SetActive(true);
    }

    public void ActivateSkillInventoryPanel()
    {
        ActivatePopupPanel();
        _activePopup = _panelSkillInventory;
        _panelSkillInventory.SetActive(!_panelSkillInventory.activeSelf);
    }

    public void ActivateGainedRecordPanel(Define.GoodsType type, float amount)
    {
        ActivatePopupPanel();
        _activePopup = _panelGainedRecord;
        UpdateGainedRecord(type, amount);
        _panelGainedRecord.SetActive(true);
    }

    public void ActivateStageInfoPanel()
    {
        _popupStageInfo.SetActive(true);
        if(PlayerManager.Instance.IsAuto)
        {
            StageManager.Instance.StageActionStatus = Define.StageActionStatus.AutoChallenge;
        }
    }

    public void ActivateNamedMonsterInfoPanel()
    {
        _popupNamedMonsterInfo.SetActive(true);
    }
    #endregion

    #region Deactivate UI
    void DeactivatePopup()
    {
        _activePopup.SetActive(false);
        _popupPanel.SetActive(false);
        _activePopup = null;
    }

    private void DeactivateStageInfoPanel()
    {
        _popupStageInfo?.SetActive(false);
    }

    public void DeactivateNamedMonsterInfo()
    {
        _popupNamedMonsterInfo.SetActive(false);
    }
    #endregion

    public void UpdateGainedRecord(Define.GoodsType type, float amount)
    {
        _panelGainedRecord.GetComponent<PopupUI_GainRecord>().SetGoods(type, amount);
    }

    public void UpdateGainedRecordTime()
    {
        _panelGainedRecord.GetComponent<PopupUI_GainRecord>().SetTime();
    }
}
