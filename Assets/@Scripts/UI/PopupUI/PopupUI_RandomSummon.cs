using System;
using UnityEngine;
using UnityEngine.UI;

// * 랜덤 뽑기 패널 스크립트
public class PopupUI_RandomSummon : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _equipmentItemButton;
    [SerializeField] Button _skillItemButton;
    [SerializeField] Button _drawOneTimeEquipmentItemButton;
    [SerializeField] Button _drawTenTimeEquipmentItemButton;
    [SerializeField] Button _drawOneTimeSkillButton;
    [SerializeField] Button _drawTenTimeSkillButton;
    [SerializeField] GameObject _equipmentGachaPanel;
    [SerializeField] GameObject _skillGachaPanel;
    [SerializeField] GameObject _resultPanel;

    public event Action OnExitButtonClicked;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _resultPanel.SetActive(false);
        OnEquipmentGachaPanel();
    }

    private void Initialize()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _equipmentItemButton.onClick.AddListener(OnEquipmentGachaPanel);
        _skillItemButton.onClick.AddListener(OnSkillGachaPanel);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }

    void OnEquipmentGachaPanel()
    {
        _equipmentGachaPanel.SetActive(true);
        _skillGachaPanel.SetActive(false);
    }

    void OnSkillGachaPanel()
    {
        _equipmentGachaPanel.SetActive(false);
        _skillGachaPanel.SetActive(true);
    }

    void OnClickDrawButton(int drawCount, Define.DrawItemType drawItemType)
    {

    }
}
