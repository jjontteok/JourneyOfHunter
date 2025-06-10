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

    bool _isAuto;

    public bool IsAuto
    {
        get { return _isAuto; }
        set { _isAuto = value; }
    }

    public Action<float> OnShortestSkillDistanceChanged;

    public BasicSkillSlot BasicSkillSlot
    {
        get { return _basicSkillSlot; }
    }

    public void InitializeSkillSystem()
    {
        _player = FindAnyObjectByType<PlayerController>();
        OnShortestSkillDistanceChanged += _player.SetShortestSkillDistance;

        Dictionary<string, GameObject> skillList = ObjectManager.Instance.PlayerSkillResourceList;
        foreach (var skillObject in skillList)
        {
            _skillList.Add(skillObject.Value.GetComponent<Skill>());
        }

        foreach (var skill in _skillList)
        {
            AddSkill(skill.SkillData);
            if (_activeSkillSlotList.Count == _player.PlayerData.UnlockedSkillSlotCount)
                break;
        }
    }

    private void Update()
    {
        if (_isAuto)
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
                    if (slot != null)
                    {
                        slot.ActivateSlotSkill();
                    }
                }
            }
        }
        else
        {
            // 기본 공격만 알아서 되도록
            _basicSkillSlot.ActivateSlotSkill();
        }
    }

    bool IsBasicAttackPossible()
    {
        // 모든 스킬이 쿨타임 중이거나 마나 부족일 때
        return _activeSkillSlotList.All(slot => !slot || !slot.IsActivatePossible || _player.MP < slot.SkillData.MP);
    }

    public void AddSkill(SkillData data)
    {
        SkillSlot skillSlot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
        if (skillSlot != null)
        {
            Debug.Log($"Skill named {data.skillName} already exists!!!");
            return;
        }

        GameObject go = new GameObject(data.name + " slot");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.up;
        // 기본 공격이면 기본공격 슬롯 따로 만들어서 저장 및 관리
        if (data.name == "PlayerBasicAttack")
        {
            _basicSkillSlot = go.AddComponent<BasicSkillSlot>();
            _basicSkillSlot.SetSkill(data);
        }
        // 패시브면 효과 적용만 시키고
        // 나중에 추가
        // 액티브면 슬롯 만들어서 저장 및 관리
        else
        {
            SkillSlot slot = go.AddComponent<SkillSlot>();
            // 비어있는 인덱스 있나 찾아보고
            int idx = _activeSkillSlotList.FindIndex((slot) => slot == null);
            if (idx >= _player.PlayerData.UnlockedSkillSlotCount)
            {
                Debug.Log("Slot list is already full!!!");
            }
            // 없으면 add
            if (idx < 0)
            {
                _activeSkillSlotList.Add(slot);
                SkillManager.Instance.SubscribeEvents(slot, _activeSkillSlotList.Count - 1);
            }
            else
            {
                _activeSkillSlotList[idx] = slot;
                SkillManager.Instance.SubscribeEvents(slot, idx);
            }
            slot.SetSkill(data);
            GetShortestSkillDistance();
        }
    }

    public void RemoveSkill(SkillData data)
    {
        SkillSlot slot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
        if (slot == null)
        {
            Debug.Log("Cannot Find Skill with name " + data.skillName);
            return;
        }
        slot.DestroySkillSlot();
        GetShortestSkillDistance();
    }

    public float GetShortestSkillDistance()
    {
        float min = _activeSkillSlotList[0].SkillData.targetDistance;
        for (int i = 1; i < _activeSkillSlotList.Count; i++)
        {
            if (min > _activeSkillSlotList[i].SkillData.targetDistance)
                min = _activeSkillSlotList[i].SkillData.targetDistance;
        }

        OnShortestSkillDistanceChanged?.Invoke(min);
        return min;
    }
}
