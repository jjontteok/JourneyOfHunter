using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static PopupUI_SkillInventory MouseOverInventory; // 마우스가 올라간 인벤토리
    public static GameObject SlotHoveredOver;   // 마우스 커서가 위치한 슬롯
    public static GameObject DragImage;         // 드래그 중인 아이템 이미지
}

[RequireComponent(typeof(EventTrigger))]
public class PopupUI_SkillInventory : MonoBehaviour
{
    [SerializeField] GameObject _slot;
    [SerializeField] GameObject _generalSkillScrollView;
    [SerializeField] GameObject _ultimateSkillScrollView;
    [SerializeField] GameObject _passiveSkillScrollView;

    [SerializeField] Transform _generalSkillContainer;
    [SerializeField] Transform _ultimateSkillContainer;
    [SerializeField] Transform _passiveSkillContainer;

    [SerializeField] Button _equipButton;
    [SerializeField] Button _releaseButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _generalButton;
    [SerializeField] Button _ultimateButton;
    [SerializeField] Button _passiveButton;

    [SerializeField] TMP_Text _skillNameText;
    [SerializeField] TMP_Text _skillDescriptionText;
    [SerializeField] TMP_Text _skillAttributeText;
    [SerializeField] Image _skillIconImage;
    [SerializeField] Image[] _currentSkillIconImage = new Image[6];

    SkillItemSlot[] _slots = new SkillItemSlot[24];
    SkillItemSlot[] _ultimateSlots = new SkillItemSlot[12];
    SkillItemSlot[] _passiveSlots = new SkillItemSlot[12];

    SkillData _selectedSkillData;
    int skillCount = 0;
    int ultimateSkillCount = 0;
    int passiveSkillCount = 0;

    Dictionary<GameObject, SkillItemSlot> _slotUIs = new Dictionary<GameObject, SkillItemSlot>();

    public Action<SkillData> OnUseSkillItem;
    // 스킬 장착 시 이벤트
    public Action<SkillData> OnEquipSkill;
    // 스킬 해제 시 이벤트
    public Action<SkillData> OnReleaseSkill;
    public event Action OnExitButtonClicked;

    public static Action<Sprite[]> OnCurrentSkillIconSet;

    #region Events
    void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = go.GetComponent<EventTrigger>();
        if (!trigger) return;
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public virtual void OnEnterInterface(GameObject go)
    {
        MouseData.MouseOverInventory = go.GetComponent<PopupUI_SkillInventory>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.MouseOverInventory = null;
    }

    public void OnEnterSlot(GameObject go)
    {
        MouseData.SlotHoveredOver = go;
    }

    public void OnLeftClick(SkillItemSlot slot)
    {
        ActivateDescription(slot.SkillData);
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        SkillItemSlot slot = _slotUIs[go];
        if (slot == null)
            return;
        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
    }
    #endregion

    // 처음 게임 시작 시, SkillItemSlot 생성 및 보유 중인 스킬 업데이트
    void CreateSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _generalSkillContainer);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _slots[i] = go.GetComponent<SkillItemSlot>();
            _slots[i].UpdateSlot();

            _slotUIs.Add(go, _slots[i]);
            go.name = "SkillItemSlot " + i;
        }
        for (int i = 0; i < _ultimateSlots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _ultimateSkillContainer);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _ultimateSlots[i] = go.GetComponent<SkillItemSlot>();
            _ultimateSlots[i].UpdateSlot();

            _slotUIs.Add(go, _ultimateSlots[i]);
            go.name = "UltimateSkillItemSlot " + i;
        }
        for (int i = 0; i < _passiveSlots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _passiveSkillContainer);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _passiveSlots[i] = go.GetComponent<SkillItemSlot>();
            _passiveSlots[i].UpdateSlot();

            _slotUIs.Add(go, _passiveSlots[i]);
            go.name = "PassiveSkillItemSlot " + i;
        }

        // 플레이어가 보유 중인 스킬리스트를 스킬 인벤토리에 등록
        foreach (var skill in PlayerManager.Instance.SkillSystem.SkillList)
        {
            UpdateSkillItemSlotList(skill.SkillData);
        }
    }

    void UpdateSkillItemSlotList(SkillData skillData)
    {
        // 기본 공격은 등록하지 않음
        if (skillData.Name == "PlayerBasicAttack")
            return;
        SkillItemSlot slot;
        // 궁극기는 궁극기 인벤토리 영역에 따로 저장
        if (skillData.IsUltimate)
        {
            slot = _ultimateSlots.FirstOrDefault(s => s.SkillData?.Name == skillData.Name);
            // 새로운 스킬일 경우
            if (slot == null)
            {
                _ultimateSlots[ultimateSkillCount++].UpdateSlot(skillData);
            }
        }
        else if (skillData.IsPassive)
        {
            slot = _passiveSlots.FirstOrDefault(s => s.SkillData?.Name == skillData.Name);
            if (slot == null)
            {
                _passiveSlots[passiveSkillCount++].UpdateSlot(skillData);
            }
        }
        else
        {
            slot = _slots.FirstOrDefault(s => s.SkillData?.Name == skillData.Name);
            if (slot == null)
            {
                _slots[skillCount++].UpdateSlot(skillData);
            }
        }
    }

    private void OnEnable()
    {
        // 스킬 설명 초기화
        ActivateDescription();
        // 일반스킬 뷰포트 활성화
        OnGeneralButtonClick();
    }

    private void Awake()
    {
        Initialize();

        OnCurrentSkillIconSet += UpdateCurrentSkillIcon;
    }

    private void OnDestroy()
    {
        OnCurrentSkillIconSet -= UpdateCurrentSkillIcon;
    }

    void Initialize()
    {
        CreateSlot();
        _equipButton.onClick.AddListener(OnEquipButtonClick);
        _releaseButton.onClick.AddListener(OnReleaseButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _generalButton.onClick.AddListener(OnGeneralButtonClick);
        _ultimateButton.onClick.AddListener(OnUltimateButtonClick);
        _passiveButton.onClick.AddListener(OnPassiveButtonClick);

        // 스킬 장착 및 해제 이벤트 구독?
        SkillSystem skillSystem = PlayerManager.Instance.SkillSystem;
        OnEquipSkill += skillSystem.AddSkill;
        OnReleaseSkill += skillSystem.RemoveSkill;
        skillSystem.OnSkillSummon += UpdateSkillItemSlotList;

        UpdateCurrentSkillIcon(SkillManager.Instance.CurrentSkillIcons());
    }

    void ActivateDescription(SkillData data = null)
    {
        bool isEmpty = data == null;
        _selectedSkillData = data;
        _skillNameText.text = isEmpty ? string.Empty : data.Name;
        _skillDescriptionText.text = isEmpty ? string.Empty : data.Description;
        _skillAttributeText.text = isEmpty ? string.Empty : $"속성: {Define.SkillAttributes[(int)data.SkillAttribute]}\t/\t개수: {data.Count}";
        _skillIconImage.sprite = isEmpty ? null : data.IconImage;
        _skillIconImage.color = isEmpty ? Color.clear : Color.white;
        // 액티브스킬에 대해서만 장착 및 해제 버튼 활성화
        if (data != null && !data.IsPassive)
        {
            _equipButton.gameObject.SetActive(true);
            _releaseButton.gameObject.SetActive(true);
        }
        else
        {
            _equipButton.gameObject.SetActive(false);
            _releaseButton.gameObject.SetActive(false);
        }
    }

    void OnEquipButtonClick()
    {
        // 현재 스킬 슬롯에 이미 있는 스킬이면 경고문?
        // 현재 스킬 슬롯에 없는 스킬이면 장착
        OnEquipSkill?.Invoke(_selectedSkillData);
        SkillManager.Instance.UpdateEnhancedAttribute(EnvironmentManager.Instance.CurrentType);
    }

    #region ButtonEvents
    void OnReleaseButtonClick()
    {
        // 현재 스킬 슬롯에 있는 스킬이면 해제
        // 현재 스킬 슬롯에 없는 스킬이면 경고문?
        OnReleaseSkill?.Invoke(_selectedSkillData);
        SkillManager.Instance.UpdateEnhancedAttribute(EnvironmentManager.Instance.CurrentType);
    }
    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }

    void OnGeneralButtonClick()
    {
        _generalSkillScrollView.gameObject.SetActive(true);
        _ultimateSkillScrollView.gameObject.SetActive(false);
        _passiveSkillScrollView.gameObject.SetActive(false);
    }

    void OnUltimateButtonClick()
    {
        _generalSkillScrollView.gameObject.SetActive(false);
        _ultimateSkillScrollView.gameObject.SetActive(true);
        _passiveSkillScrollView.gameObject.SetActive(false);
    }

    void OnPassiveButtonClick()
    {
        _generalSkillScrollView.gameObject.SetActive(false);
        _ultimateSkillScrollView.gameObject.SetActive(false);
        _passiveSkillScrollView.gameObject.SetActive(true);
    }

    void UpdateCurrentSkillIcon(Sprite[] sprites)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            _currentSkillIconImage[i].sprite = sprites[i];
            _currentSkillIconImage[i].color = sprites[i] == null ? Color.clear : Color.white;
        }
    }
    #endregion
}
