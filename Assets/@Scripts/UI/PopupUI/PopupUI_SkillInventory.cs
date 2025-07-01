using System;
using System.Collections.Generic;
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
    [SerializeField] GameObject _skillViewPort;
    [SerializeField] GameObject _ultimateSkillViewPort;
    [SerializeField] Button _exitButton;
    [SerializeField] SkillDescriptionPanel _skillDescriptionPanel;

    SkillItemSlot[] _slots = new SkillItemSlot[24];
    SkillItemSlot[] _ultimateSlots = new SkillItemSlot[4];

    Dictionary<GameObject, SkillItemSlot> _slotUIs = new Dictionary<GameObject, SkillItemSlot>();

    public Action<SkillData> OnUseSkillItem;
    public event Action OnExitButtonClicked;

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
        // 게임 설명 칸 등장
        _skillDescriptionPanel.TurnOnDescription(slot.SkillData);
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

    void CreateSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _skillViewPort.transform);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _slots[i] = go.GetComponent<SkillItemSlot>();

            _slotUIs.Add(go, _slots[i]);
            go.name = "SkillItemSlot " + i;
        }
        for (int i = 0; i < _ultimateSlots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _skillViewPort.transform);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _ultimateSlots[i] = go.GetComponent<SkillItemSlot>();

            _slotUIs.Add(go, _ultimateSlots[i]);
            go.name = "UltimateSkillItemSlot " + i;
        }

        int skillCount = 0;
        int ultimateSkillCount = 0;
        // 플레이어가 보유 중인 스킬리스트를 스킬 인벤토리에 등록
        foreach (var skill in PlayerManager.Instance.SkillSystem.SkillList)
        {
            // 기본 공격은 등록하지 않음
            if (skill.SkillData.SkillName == "PlayerBasicAttack")
                continue;
            // 궁극기는 궁극기 인벤토리 영역에 따로 저장
            if (skill.SkillData.IsUltimate)
            {
                _ultimateSlots[ultimateSkillCount++].UpdateSlot(skill.SkillData);
            }
            else
            {
                _slots[skillCount++].UpdateSlot(skill.SkillData);
            }
        }
    }

    private void Start()
    {
        CreateSlot();
        _exitButton.onClick.AddListener(OnExitButtonClick);

        // 스킬 장착 및 해제 이벤트 구독?
        SkillSystem skillSystem = PlayerManager.Instance.SkillSystem;
        _skillDescriptionPanel.OnEquipSkill += skillSystem.AddSkill;
        _skillDescriptionPanel.OnReleaseSkill += skillSystem.RemoveSkill;
    }

    private void OnDisable()
    {
        _skillDescriptionPanel.gameObject.SetActive(false);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}
