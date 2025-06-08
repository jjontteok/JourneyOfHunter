using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] Button _statusButton;
    [SerializeField] Button _inventoryButton;
    [SerializeField] Toggle _autoToggle;
    [SerializeField] TMP_Text _timeText;

    private int _minute;
    private int _second;

    public Action<bool> OnAutoChanged;


    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        TimeManager.Instance.OnTimeChanged += SetTimeText;
        _statusButton.onClick.AddListener(OnStatusButtonClick);
        _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
        // _inventoryButton.onClick.AddListener(OnInventoryButtonClick);

        OnAutoChanged += FindAnyObjectByType<PlayerController>().SetAuto;
    }

    //private void OnEnable()
    //{
    //    TimeManager.Instance.OnTimeChanged += SetTimeText;
    //    _statusButton.onClick.AddListener(OnStatusButtonClick);
    //    _autoToggle.onValueChanged.AddListener(OnAutoToggleClick);
    //   // _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
    //}

    private void OnDisable()
    {
        //TimeManager.Instance.OnTimeChanged -= SetTimeText;
    }

    void SetTimeText(float time)
    {
        _minute = (int)time / 60;
        _second = (int)time % 60;
        _timeText.text = _minute.ToString("00") + " : " + _second.ToString("00");
    }

    void OnStatusButtonClick()
    {
        PopupUIManager.Instance.ActivateStatusPanel();
    }

    void OnInventoryButtonClick()
    {
        PopupUIManager.Instance.ActivateInventoryPanel();
    }

    void OnAutoToggleClick(bool flag)
    {
        Debug.Log($"Auto: {flag}");
        OnAutoChanged?.Invoke(flag);
    }
}
