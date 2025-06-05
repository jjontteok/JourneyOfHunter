using extension;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StatusSlot : MonoBehaviour
{
    [SerializeField] StatusSlotData _statusSlotData;
    [SerializeField] TMP_Text _statusNameText;
    [SerializeField] TMP_Text _currentStatusText;
    [SerializeField] TMP_Text _upgradeCostText;
    EventTrigger _eventTrigger;
    int _level;

    bool _isButtonDown = false;
    public Button upgradeButton;

    public StatusSlotData StatusSlotData
    {
        get => _statusSlotData; set => _statusSlotData = value;
    }

    public TMP_Text CurrentStatusText
    {
        get => _currentStatusText;
        set
        {
            _currentStatusText = value;
        }
    }

    public TMP_Text UpgradeCostText
    {
        get => _upgradeCostText;
        set
        {
            _upgradeCostText = value;
        }
    }

    public int Level
    {
        get => _level; set => _level = value;
    }

    private void Awake()
    {
        _eventTrigger = upgradeButton.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((eventData) => { _isButtonDown = true; });

        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((eventData) => { _isButtonDown = false; });

        _eventTrigger.triggers.Add(entryPointerDown);
        _eventTrigger.triggers.Add(entryPointerUp);
    }

    private void OnEnable()
    {
        StatusSlotData.OnUpgradeStatus += UpgradeStatus;
    }

    private void OnDisable()
    {
        StatusSlotData.OnUpgradeStatus -= UpgradeStatus;
    }

    private void Update()
    {
        if (_isButtonDown)
        {
            _level = ++_statusSlotData.level;
            _upgradeCostText.text = (++_statusSlotData.upgradeCost).ToString();
            _currentStatusText.text = (++_statusSlotData.currentStatusCount).ToString();
        }
    }

    void UpgradeStatus(string statusName, int level)
    {
        if(statusName == _statusSlotData.name)
        {
            _level = _statusSlotData.level;
            _upgradeCostText.text = (++_statusSlotData.upgradeCost).ToString();
            _currentStatusText.text = (++_statusSlotData.currentStatusCount).ToString();
        }
    }
}
