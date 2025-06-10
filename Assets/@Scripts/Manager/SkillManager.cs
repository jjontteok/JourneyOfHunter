using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillManager : Singleton<SkillManager>
{
    SkillIconSlot[] _iconSlots;

    public SkillIconSlot[] IconSlots { get { return _iconSlots; } }

    protected override void Initialize()
    {
        base.Initialize();
        _iconSlots = FindObjectsByType<SkillIconSlot>(FindObjectsSortMode.None);
        Array.Sort(_iconSlots, (slot1, slot2) => String.Compare(slot1.name, slot2.name));

        for (int i = 0; i < IconSlots.Length; i++)
        {
            int idx = i;
            AddEvent(_iconSlots[idx].gameObject, EventTriggerType.PointerClick, (data) => { OnClick(_iconSlots[idx].gameObject, (PointerEventData)data); });
        }
    }

    public void LockIconSlots(int idx)
    {
        for (int i = idx; i < _iconSlots.Length; i++)
        {
            _iconSlots[i].LockIconSlot();
        }
    }

    public void SubscribeEvents(SkillSlot skillSlot, int idx)
    {
        skillSlot.OnGenerateSlot += _iconSlots[idx].SetIconSlot;
        skillSlot.OnActivateSkill += _iconSlots[idx].StartIconCoolTime;
        skillSlot.OnRemoveSkill += _iconSlots[idx].ReleaseIconSlot;

        _iconSlots[idx].OnClickSkillIcon = skillSlot.ActivateSlotSkill;
    }

    #region Events
    void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = go.GetComponent<EventTrigger>();
        if (!trigger) return;
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnLeftClick(SkillIconSlot slot)
    {
        //해당 아이콘의 스킬 실행
        slot.OnClickSkillIcon?.Invoke();
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        SkillIconSlot slot = go.GetComponent<SkillIconSlot>();
        if (slot == null)
            return;
        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
    }
    #endregion
}
