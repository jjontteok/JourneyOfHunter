using System;
using UnityEngine;
using UnityEngine.UI;
using extension;

// * 팝업 UI 매니저 스크립트
//- 특정 이벤트 발생 시 팝업 UI 생성
public class PopupUIManager : Singleton<PopupUIManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _canvasPopupUI;
    private GameObject _panelEnterDungeon;
    private GameObject _panelStatus;
    private GameObject _panelInventory;
    private GameObject _panelSkillInventory;
    private GameObject _panelGainedRecord;
    private GameObject _panelStageInfo;

    public Button ButtonEnterDungeon;

    public event Action OnButtonDungeonEnterClick;

    public GameObject PanelGainedRecord
    {
        get { return _panelGainedRecord; }
    }

    #region Initialize
    protected override void Initialize()
    {
        _canvasPopupUI = Instantiate(ObjectManager.Instance.PopupCanvas);

        _panelEnterDungeon = Instantiate(ObjectManager.Instance.PopupPanel, _canvasPopupUI.transform);
        _panelStatus = Instantiate(ObjectManager.Instance.PopupStatusPanel, _canvasPopupUI.transform);
        _panelInventory = Instantiate(ObjectManager.Instance.PopupInventoryPanel, _canvasPopupUI.transform);
        _panelSkillInventory = Instantiate(ObjectManager.Instance.PopupSkillInventory, _canvasPopupUI.transform);
        _panelGainedRecord = Instantiate(ObjectManager.Instance.PopupGainedRecordPanel, _canvasPopupUI.transform);
        _panelStageInfo = Instantiate(ObjectManager.Instance.PopupStageInfoPanel, _canvasPopupUI.transform);

        ButtonEnterDungeon = _panelEnterDungeon.GetComponentInChildren<Button>();
    }
    #endregion


    #region IEventSubscriber
    public void Subscribe()
    {
        ButtonEnterDungeon.onClick.AddListener(OnDungeonEnter);
        DungeonManager.Instance.OnDungeonEnter += ActivateStageInfoPanel;
        DungeonManager.Instance.OnDungeonExit += DeactivateStageInfoPanel;
        //DungeonManager.Instance.OnSpawnableNamedMonster += 
    }
    #endregion

    #region IDeactivate
    public void Deactivate()
    {
        _panelEnterDungeon.SetActive(false);
        _panelStatus.SetActive(false);
        _panelInventory.SetActive(false);
        _panelSkillInventory.SetActive(false);
        _panelGainedRecord.SetActive(false);
        _panelStageInfo.SetActive(false);
    }
    #endregion

    private void PopupUI()
    {
        _panelEnterDungeon.SetActive(true);
    }

    private void OnDungeonEnter()
    {
        OnButtonDungeonEnterClick?.Invoke();
        _panelEnterDungeon.SetActive(false);
    }

    public void ActivateStatusPanel()
    {
        _panelStatus.SetActive(true);
    }

    public void ActivateInventoryPanel()
    {
        _panelInventory.SetActive(true);
    }

    public void ActivateSkillInventoryPanel()
    {
        _panelSkillInventory.SetActive(!_panelSkillInventory.activeSelf);
    }

    public void ActivateGainedRecordPanel(Define.GoodsType type, float amount)
    {
        UpdateGainedRecord(type, amount);
        _panelGainedRecord.SetActive(true);
    }

    public void ActivateStageInfoPanel()
    {
        _panelStageInfo.SetActive(true);
    }

    public void UpdateGainedRecord(Define.GoodsType type, float amount)
    {
        _panelGainedRecord.GetComponent<UI_GainRecord>().SetGoods(type, amount);
    }

    private void DeactivateStageInfoPanel()
    {
        _panelStageInfo?.SetActive(false);
    }
}
