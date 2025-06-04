using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    SkillIconSlot[] _iconSlots;

    public SkillIconSlot[] IconSlots { get { return _iconSlots; } }

    protected override void Initialize()
    {
        base.Initialize();
        _iconSlots = FindObjectsByType<SkillIconSlot>(FindObjectsSortMode.None);
        Array.Sort(_iconSlots, (slot1, slot2) => String.Compare(slot1.name, slot2.name));
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
    }
}
