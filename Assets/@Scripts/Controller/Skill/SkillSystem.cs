using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //플레이어의 스킬 리스트 - 스킬 슬롯
    List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트 - 액티브형 스킬 보관 슬롯
    List<SkillSlot> _activeSkillSlotList = new List<SkillSlot>();
    BasicSkillSlot _basicSkillSlot;
    PlayerController _player;

    public BasicSkillSlot BasicSkillSlot
    {
        get { return _basicSkillSlot; }
    }

    private void Awake()
    {
        //InitializeSkillSystem();
    }

    public void InitializeSkillSystem()
    {
        _player = FindAnyObjectByType<PlayerController>();

        Dictionary<string, GameObject> skillList = ObjectManager.Instance.PlayerSkillResourceList;
        foreach (var skillObject in skillList)
        {
            _skillList.Add(skillObject.Value.GetComponent<Skill>());
        }

        foreach (var skill in _skillList)
        {
            // 패시브면 효과 적용만 시키고
            // 나중에 추가
            GameObject go = new GameObject(skill.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up;
            // 기본 공격이면 기본공격 슬롯 따로 만들어서 저장 및 관리
            if (skill.SkillData.name == "PlayerBasicAttack")
            {
                _basicSkillSlot = go.AddComponent<BasicSkillSlot>();
                _basicSkillSlot.SetSkill(skill);
            }
            // 액티브면 슬롯 만들어서 저장 및 관리
            else
            {
                SkillSlot slot = go.AddComponent<SkillSlot>();
                slot.SetSkill(skill);
                _activeSkillSlotList.Add(slot);
            }
        }
    }

    private void Update()
    {
        // 기본 공격할 타이밍인지 체크
        if (IsBasicAttackPossible())
        {
            _basicSkillSlot.ActivateSlotSkill();
        }
        else
        {
            foreach (var slot in _activeSkillSlotList)
            {
                slot.ActivateSlotSkill();
            }
        }
    }

    bool IsBasicAttackPossible()
    {
        // 모든 스킬이 쿨타임 중이거나 마나 부족일 때
        return _activeSkillSlotList.All(slot => !slot.IsActivatePossible || _player.MP < slot.SkillData.MP);
    }
}
