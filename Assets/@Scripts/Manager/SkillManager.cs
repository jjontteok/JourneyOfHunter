using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillManager : Singleton<SkillManager>
{
    SkillIconSlot[] _skillIconSlots;

    bool _isSkillInterval;

    public bool IsSkillInterval
    {
        get
        {
            return _isSkillInterval;
        }
        set
        {
            // interval이 true가 되면 푸른 칸 효과로
            // interval이 false가 되면 원래대로
            _isSkillInterval = value;
            OnOffSkillIntervalImage(value);
        }
    }

    public SkillIconSlot[] IconSlots { get { return _skillIconSlots; } }

    //protected override void Initialize()
    //{
    //    base.Initialize();
    //    _skillIconSlots = FindObjectsByType<SkillIconSlot>(FindObjectsSortMode.None);
    //    Array.Sort(_skillIconSlots, (slot1, slot2) => String.Compare(slot1.name, slot2.name));

    //    for (int i = 0; i < IconSlots.Length; i++)
    //    {
    //        int idx = i;
    //        AddEvent(_skillIconSlots[idx].gameObject, EventTriggerType.PointerClick, (data) => { OnClick(_skillIconSlots[idx].gameObject, (PointerEventData)data); });
    //    }
    //}

    // Panel - SkillIconSlot에 생성된 슬롯들 가져오기
    public void SetIconSlots(SkillIconSlot[] slots)
    {
        _skillIconSlots = slots;
    }

    public void LockIconSlots(int idx)
    {
        for (int i = idx; i < _skillIconSlots.Length; i++)
        {
            _skillIconSlots[i].LockIconSlot();
        }
    }

    public void SubscribeEvents(SkillSlot skillSlot, int idx)
    {
        skillSlot.OnGenerateSlot += _skillIconSlots[idx].SetIconSlot;
        skillSlot.OnActivateSkill += _skillIconSlots[idx].StartIconCoolTime;
        skillSlot.OnRemoveSkill += _skillIconSlots[idx].ReleaseIconSlot;

        _skillIconSlots[idx].OnClickSkillIcon = () =>
        {
            if (!IsSkillInterval)
            {
                skillSlot.ActivateSlotSkill();
            }
        };
    }

    public void OnOffSkillIntervalImage(bool flag)
    {
        foreach (SkillIconSlot skillSlot in _skillIconSlots)
        {
            skillSlot.OnOffSkillIntervalImage(flag);
        }
    }

    //#region Events
    //void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    //{
    //    var trigger = go.GetComponent<EventTrigger>();
    //    if (!trigger) return;
    //    EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
    //    eventTrigger.callback.AddListener(action);
    //    trigger.triggers.Add(eventTrigger);
    //}

    //public void OnLeftClick(SkillIconSlot slot)
    //{
    //    //해당 아이콘의 스킬 실행
    //    slot.OnClickSkillIcon?.Invoke();
    //}

    //public void OnClick(GameObject go, PointerEventData data)
    //{
    //    SkillIconSlot slot = go.GetComponent<SkillIconSlot>();
    //    if (slot == null)
    //        return;
    //    if (data.button == PointerEventData.InputButton.Left)
    //    {
    //        OnLeftClick(slot);
    //    }
    //}
    //#endregion
}
