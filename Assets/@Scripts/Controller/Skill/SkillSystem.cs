using System;
using System.Collections.Generic;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //플레이어의 스킬 리스트 - 스킬 슬롯
    [SerializeField] List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트 - 액티브형 스킬 보관 슬롯
    public List<SkillSlot> _slotList = new List<SkillSlot>();

    public void InitializeSkillSystem()
    {
        foreach(var skill in _skillList)
        {
            // 패시브면 효과 적용만 시키고
            // 액티브면 슬롯 만들어서 저장 및 관리
            GameObject go = new GameObject(skill.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up;
            SkillSlot slot = go.AddComponent<SkillSlot>();
            slot.SetSkill(skill);
            _slotList.Add(slot);
        }
    }
}
