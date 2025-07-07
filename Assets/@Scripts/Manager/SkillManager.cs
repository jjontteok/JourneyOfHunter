using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillManager : Singleton<SkillManager>
{
    SkillIconSlot[] _skillIconSlots;
    UltimateSkillIconSlot _ultimateSkillIconSlot;
    UIEffectsManager[] _iconEffects = new UIEffectsManager[6];

    bool _isSkillInterval;

    public event Action<Define.TimeOfDayType> OnDayTypeChanged;

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

    public UltimateSkillIconSlot UltimateIconSlot { get { return _ultimateSkillIconSlot; } }

    public Sprite[] CurrentSkillIcons()
    {
        Sprite[] sprites = new Sprite[6];
        sprites[0] = _ultimateSkillIconSlot.SkillIconSprite;
        for (int i = 1; i < sprites.Length; i++)
        {
            sprites[i] = _skillIconSlots[i - 1].SkillIconSprite;
        }
        return sprites;
    }

    // Panel - SkillIconSlot에 생성된 슬롯들 가져오기
    public void SetIconSlots(SkillIconSlot[] slots, UltimateSkillIconSlot ultimate)
    {
        _skillIconSlots = slots;
        _ultimateSkillIconSlot = ultimate;
        for (int i = 0; i < _skillIconSlots.Length; i++)
        {
            _iconEffects[i] = _skillIconSlots[i].GetComponent<UIEffectsManager>();
        }
        _iconEffects[5] = _ultimateSkillIconSlot.GetComponent<UIEffectsManager>();

        EnvironmentManager.Instance.OnTypeChanged += UpdateEnhancedAttribute;
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
        if (idx < 0)
        {
            skillSlot.OnGenerateSlot += _ultimateSkillIconSlot.SetIconSlot;
            skillSlot.OnActivateSkill += _ultimateSkillIconSlot.StartIconCoolTime;
            skillSlot.OnRemoveSkill += _ultimateSkillIconSlot.ReleaseIconSlot;

            _ultimateSkillIconSlot.OnClickSkillIcon = () =>
            {
                if (!IsSkillInterval)
                {
                    skillSlot.ActivateSlotSkill();
                }
            };
        }
        else
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
    }

    public void OnOffSkillIntervalImage(bool flag)
    {
        foreach (SkillIconSlot skillSlot in _skillIconSlots)
        {
            skillSlot.OnOffSkillIntervalImage(flag);
        }
        _ultimateSkillIconSlot.OnOffSkillIntervalImage(flag);
    }

    public void UpdateEnhancedAttribute(Define.TimeOfDayType type)
    {
        for (int i = 0; i < _iconEffects.Length; i++)
        {
            if (i == _iconEffects.Length - 1)
            {
                _ultimateSkillIconSlot.UpdateAttributeEffect(type);
            }
            else
            {
                _skillIconSlots[i].UpdateAttributeEffect(type);
            }
        }
    }
}
