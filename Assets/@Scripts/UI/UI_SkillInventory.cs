using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static UI_SkillInventory MouseOverInventory; // 마우스가 올라간 인벤토리
    public static GameObject SlotHoveredOver;   // 마우스 커서가 위치한 슬롯
    public static GameObject DragImage;         // 드래그 중인 아이템 이미지
}

[RequireComponent(typeof(EventTrigger))]
public class UI_SkillInventory : MonoBehaviour
{
    [SerializeField] GameObject _slot;
    [SerializeField] GameObject _viewPort;
    [SerializeField] Button _exitButton;
    [SerializeField] SkillDescriptionPanel _skillDescriptionPanel;

    SkillItemSlot[] _slots = new SkillItemSlot[24];

    Dictionary<GameObject, SkillItemSlot> _slotUIs = new Dictionary<GameObject, SkillItemSlot>();

    public Action<SkillData> OnUseSkillItem;

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
        MouseData.MouseOverInventory = go.GetComponent<UI_SkillInventory>();
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

    // 슬롯의 아이템 사용하기
    public void UseItem(SkillItemSlot slot)
    {
        if (slot.SkillData == null)
        {
            return;
        }
        SkillData skillData = slot.SkillData;
        OnUseSkillItem?.Invoke(skillData);
    }

    void CreateSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject go = Instantiate(_slot, _viewPort.transform);

            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            _slots[i] = go.GetComponent<SkillItemSlot>();

            _slotUIs.Add(go, _slots[i]);
            go.name = "SkillItemSlot " + i;
        }
        int j = 0;
        foreach (var skillResource in ObjectManager.Instance.PlayerSkillResourceList)
        {
            Skill skill = skillResource.Value.GetComponent<Skill>();
            if (skill.SkillData.skillName == "PlayerBasicAttack")
                continue;
            _slots[j++].UpdateSlot(skill.SkillData);
        }
    }

    private void Start()
    {
        CreateSlot();
        _exitButton.onClick.AddListener(OnExitButtonClick);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _skillDescriptionPanel.gameObject.SetActive(false);
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
