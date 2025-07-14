using System;
using UnityEngine;
using UnityEngine.UI;
using extension;
using TMPro;
using System.Collections;

// * 팝업 UI 매니저 스크립트
//- 특정 이벤트 발생 시 팝업 UI 생성
public class PopupUIManager : Singleton<PopupUIManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _canvasPopupUI;
    private GameObject _popupPanel;
    private GameObject _toolTipPanel;
    private GameObject _popupJourneyInfo;
    private GameObject _popupStageInfo;
    private GameObject _popupNamedMonsterInfo;
    private GameObject _popupStageText;
    private GameObject _popupTreasureAppearText;
    private GameObject _popupBuffText;
    private GameObject _popupDungeonAppear;
    private GameObject _popupDungeonClearText;
    private GameObject _panelStatus;
    private GameObject _panelInventory;
    private GameObject _panelSkillInventory;
    private GameObject _panelMerchant;
    private GameObject _panelGacha;

    private GameObject _activePopup;

    WaitForSeconds _oneFiveSecondsTime = new WaitForSeconds(1.5f);
    WaitForSeconds _twoSecondsTime = new WaitForSeconds(2f);

    bool _isDownBuffTextPos = false;

    public GameObject PanelSkillInventory
    {
        get { return _panelSkillInventory; }
    }

    #region Initialize
    protected override void Initialize()
    {
        _canvasPopupUI = Instantiate(ObjectManager.Instance.PopupCanvas);
        _popupPanel = Instantiate(ObjectManager.Instance.PopupPanel, _canvasPopupUI.transform);
        _popupJourneyInfo = Instantiate(ObjectManager.Instance.PopupJourneyInfo, _canvasPopupUI.transform);
        _popupStageInfo = Instantiate(ObjectManager.Instance.PopupStageInfo, _canvasPopupUI.transform);
        _popupNamedMonsterInfo = Instantiate(ObjectManager.Instance.PopupNamedMonsterInfo, _canvasPopupUI.transform);
        _popupStageText = Instantiate(ObjectManager.Instance.PopupStageTextPanel, _canvasPopupUI.transform);
        _popupTreasureAppearText = Instantiate(ObjectManager.Instance.PopupTreasureAppearText, _canvasPopupUI.transform);
        _popupBuffText = Instantiate(ObjectManager.Instance.PopupBuffText, _canvasPopupUI.transform);
        _popupDungeonAppear = Instantiate(ObjectManager.Instance.PopupDungeonAppear, _canvasPopupUI.transform);
        _popupDungeonClearText = Instantiate(ObjectManager.Instance.PopupDungeonClearText, _canvasPopupUI.transform);
        _panelStatus = Instantiate(ObjectManager.Instance.PopupStatusPanel, _canvasPopupUI.transform);
        _panelInventory = Instantiate(ObjectManager.Instance.PopupInventoryPanel, _canvasPopupUI.transform);
        _panelSkillInventory = Instantiate(ObjectManager.Instance.PopupSkillInventory, _canvasPopupUI.transform);
        _panelMerchant = Instantiate(ObjectManager.Instance.PopupMerchantPanel, _canvasPopupUI.transform);
        _panelGacha = Instantiate(ObjectManager.Instance.PopupGachaPanel, _canvasPopupUI.transform);

        // 진짜 바보같은 코드... 가장 마지막으로보내버림
        _toolTipPanel = Instantiate(ObjectManager.Instance.ToolTipPanel, _canvasPopupUI.transform);
    }
    #endregion


    #region IEventSubscriber
    public void Subscribe()
    {
        UIManager.Instance.UI_Main.OnStartButtonClicked += ActivateJourneyInfo;
        FieldManager.Instance.OnStageChanged += ActivateStageText;
        FieldManager.Instance.DungeonController.OnDungeonEnter += DeactivateJourneyInfo;
        FieldManager.Instance.DungeonController.OnDungeonEnter += ActivateStageInfo;
        FieldManager.Instance.DungeonController.OnDungeonEnter += ActivateBuffText;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += DeactivateStageInfo;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += DeactivateBuffText;
        //FieldManager.Instance.DungeonController.OnSpawnNamedMonster += ActivateNamedMonsterInfo;
        FieldManager.Instance.DungeonController.OnDungeonExit += DeactivateNamedMonsterInfo;
        FieldManager.Instance.DungeonController.OnDungeonExit += DeactivateStageInfo;
        FieldManager.Instance.DungeonController.OnDungeonExit += DeactivateBuffText;
        FieldManager.Instance.DungeonController.OnDungeonExit += ActivateJourneyInfo;

        _popupPanel.GetComponent<PopupUI_Panel>().OnPopupPanelClicked += DeactivatePopup;
        _panelStatus.GetComponent<PopupUI_Status>().OnExitButtonClicked += DeactivatePopup;
        _panelInventory.GetComponent<PopupUI_Inventory>().OnExitButtonClicked += DeactivatePopup;
        _panelSkillInventory.GetComponent<PopupUI_SkillInventory>().OnExitButtonClicked += DeactivatePopup;
        _panelMerchant.GetComponent<PopupUI_Merchant>().OnExitButtonClicked += DeactivatePopup;
        _panelGacha.GetComponent<PopupUI_RandomSummon>().OnExitButtonClicked += DeactivatePopup;
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
        _toolTipPanel.SetActive(false);
        _popupJourneyInfo.SetActive(false);
        _popupStageInfo.SetActive(false);
        _popupNamedMonsterInfo.SetActive(false);
        _popupStageText.SetActive(false);
        _popupTreasureAppearText.SetActive(false);
        _popupBuffText.SetActive(false);
        _popupDungeonAppear.SetActive(false);
        _popupDungeonClearText.SetActive(false);
        _panelStatus.SetActive(false);
        _panelInventory.SetActive(false);
        _panelSkillInventory.SetActive(false);
        _panelMerchant.SetActive(false);
        _panelGacha.SetActive(false);
    }
    #endregion

    #region Activate UI
    void ActivatePopupPanel()
    {
        _popupPanel.SetActive(true);
    }
    public void ActivateToolTipPanel(Vector2 pos, string name, string content)
    {
        _toolTipPanel.SetActive(true);
        _toolTipPanel.GetComponent<PopupUI_ToolTip>().SetToolTip(pos, name, content);
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


    public void ActivateMerchantPanel()
    {
        ActivatePopupPanel();
        _activePopup = _panelMerchant;
        _panelMerchant.SetActive(true);
    }

    public void ActivateJourneyInfo()
    {
        _popupJourneyInfo.SetActive(true);
    }

    public void ActivateStageInfo()
    {
        _popupStageInfo.SetActive(true);
        if (PlayerManager.Instance.IsAuto)
        {
            FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.AutoChallenge;
        }
    }

    public void ActivateNamedMonsterInfo()
    {
        _popupNamedMonsterInfo.SetActive(true);
    }

    public void SetNamedMonster(float curHp, float maxHp)
    {
        _popupNamedMonsterInfo.GetComponent<PopupUI_NamedMonsterInfo>().
            SetNamedMonsterHpBar(curHp, maxHp);
    }

    public void ActivateStageText(int stage)
    {
        if (stage % 5 == 0)
        {
            _popupStageInfo.GetComponent<PopupUI_StageInfo>().Stage = stage;
            ActivateDungeonAppear();
            return;
        }
        TMP_Text stageText = _popupStageText.GetComponentInChildren<TMP_Text>();
        stageText.text = $"- Stage {stage} -";
        _popupStageText.SetActive(true);
        StartCoroutine(_popupStageText.GetComponentInChildren<UIEffectsManager>().PerformEffect(0));
        if (stage % 5 == 0)
        {
            stageText.color = Color.yellow;
            StartCoroutine(_popupStageText.GetComponentInChildren<UIEffectsManager>().PerformEffect(1));
        }
        StartCoroutine(DeactivatePopup(_popupStageText, _oneFiveSecondsTime));
    }

    public void ActivateTreasureAppearText()
    {
        if (_popupTreasureAppearText != null)
        {
            _popupTreasureAppearText.SetActive(true);
            StartCoroutine(_popupTreasureAppearText.GetComponent<UIEffectsManager>().PerformEffect(0));
            StartCoroutine(DeactivatePopup(_popupTreasureAppearText, _oneFiveSecondsTime));
        }
    }

    public void ActivateBuffText()
    {
        if (FieldManager.Instance.FailedCount != 0 && _popupBuffText != null)
        {
            _popupBuffText.SetActive(true);
        }
    }

    public void ActivateDungeonAppear()
    {
        _popupDungeonAppear.SetActive(true);
        AudioManager.PlayWarning?.Invoke(true);
        foreach (var effect in _popupDungeonAppear.GetComponentsInChildren<UIEffectsManager>())
        {
            StartCoroutine(effect.PerformEffect(0));
        }
        StartCoroutine(DeactivatePopup(_popupDungeonAppear, _twoSecondsTime));
    }

    public void ActivateDungeonClearText(bool isClear)
    {
        _popupDungeonClearText.GetComponent<PopupUI_DungeonClearText>().IsClear = isClear;
        _popupDungeonClearText.SetActive(true);
    }

    public void ActivateGachaPanel()
    {
        ActivatePopupPanel();
        _activePopup = _panelGacha;
        _panelGacha.SetActive(true);
    }
    #endregion

    //네임드 인포가 활성화 될 떄에는 위치를 옮겨줘야 해,,, 안 그러면 겹치니까
    public void ModifyBuffTextPos()
    {
        if (FieldManager.Instance.FailedCount != 0)
        {
            _popupBuffText.transform.position -= Vector3.up * 40;
            _isDownBuffTextPos = true;
            _popupBuffText.SetActive(true);
        }
    }

    void SetOriginBuffTextPos()
    {
        if (_isDownBuffTextPos)
        {
            _popupBuffText.transform.position += Vector3.up * 40;
            _isDownBuffTextPos = false;
        }
    }

    #region Deactivate UI
    void DeactivatePopup()
    {
        if (_activePopup == _panelMerchant)
        {
            PlayerManager.Instance.IsAutoMoving = true;
        }
        _activePopup.SetActive(false);
        _popupPanel.SetActive(false);
        _activePopup = null;
    }

    public void DeactivateToolTipPanel()
    {
        _toolTipPanel.SetActive(false);
    }

    public void DeactivateJourneyInfo()
    {
        _popupJourneyInfo.SetActive(false);
    }

    private void DeactivateStageInfo()
    {
        _popupStageInfo?.SetActive(false);
    }

    public void DeactivateNamedMonsterInfo()
    {
        _popupNamedMonsterInfo.SetActive(false);
    }

    public IEnumerator DeactivatePopup(GameObject go, WaitForSeconds time)
    {
        yield return time;
        go.SetActive(false);
        AudioManager.PlayWarning?.Invoke(false);
    }

    public void DeactivateBuffText()
    {
        SetOriginBuffTextPos();
        _popupBuffText.SetActive(false);
    }
    #endregion

}