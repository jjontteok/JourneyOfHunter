using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //플레이어가 보유 중인 스킬 리소스들을 담은 리스트
    List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트에 실질적으로 들어있는 스킬 개수 확인용
    // 최소 2개, 최대 5개
    int _skillSlotCount = 0;
    // 스킬 슬롯 리스트 - 액티브형 스킬 보관 슬롯
    List<SkillSlot> _activeSkillSlotList = new List<SkillSlot>();
    // 궁극기 스킬 보관 슬롯
    SkillSlot _ultimateSkillSlot;
    // 기본 공격 보관 슬롯
    BasicSkillSlot _basicSkillSlot;

    PlayerController _player;
    Animator _animator;

    public Action<float> OnShortestSkillDistanceChanged;

    public List<Skill> SkillList { get { return _skillList; } }

    public BasicSkillSlot BasicSkillSlot
    {
        get { return _basicSkillSlot; }
    }

    public List<SkillSlot> ActiveSkillSlotList
    {
        get { return _activeSkillSlotList; }
    }

    public SkillSlot UltimateSkillSlot
    {
        get { return _ultimateSkillSlot; }
    }

    public void InitializeSkillSystem()
    {
        _player = PlayerManager.Instance.Player;
        _player.OnPageUp += IncreaseUnlockedSkillSlotCount;
        _player.OnPageDown += DecreaseUnlockedSkillSlotCount;
        _animator = _player.GetComponent<Animator>();
        OnShortestSkillDistanceChanged += _player.SetShortestSkillDistance;
        _player.OnPlayerCrossed += OnPlayerCross;

        // 오브젝트매니저가 보유 중인 스킬 프리팹들을 플레이어의 스킬리스트에 넣기
        Dictionary<string, GameObject> skillList = ObjectManager.Instance.PlayerSkillResourceList;
        SkillData[] skillDatas = _player.PlayerData.CurrentSkillData;
        foreach (var skillData in skillDatas)
        {
            _skillList.Add(skillList[skillData.name].GetComponent<Skill>());
        }
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsAuto && _animator.GetInteger(Define.DieType) == 0)
        {
            // 기본 공격할 타이밍인지 체크
            if (IsBasicAttackPossible())
            {
                _basicSkillSlot.ActivateSlotSkill();
            }
            else
            {
                if (!_animator.GetBool(Define.IsAttacking))
                {
                    foreach (var slot in _activeSkillSlotList)
                    {
                        if (!SkillManager.Instance.IsSkillInterval && slot != null)
                        {
                            slot.ActivateSlotSkill();
                        }
                    }
                    if (_player.Target != null && _player.Target.gameObject.activeSelf && _player.Target.CompareTag(Define.MonsterTag) && !SkillManager.Instance.IsSkillInterval && _ultimateSkillSlot != null)
                    {
                        _ultimateSkillSlot.ActivateSlotSkill();
                    }
                }
            }
        }
    }

    bool IsBasicAttackPossible()
    {
        // skill interval이거나
        // 모든 스킬 슬롯이 쿨타임 중이거나 스킬이 없을 때
        return SkillManager.Instance.IsSkillInterval || _activeSkillSlotList.All(slot => !slot || !slot.IsActivatePossible);
    }

    public void SetSkillSlotList()
    {
        // 플레이어가 현재 보유 중인 스킬 중에서 열린 슬롯 개수만큼 가져와야함
        foreach (var skill in _skillList)
        {
            // 이미 플레이어 세팅만큼 스킬 있으면 패스
            if (!skill.SkillData.IsUltimate && _skillSlotCount == _player.PlayerData.UnlockedSkillSlotCount)
                continue;
            AddSkill(skill.SkillData);
        }
    }

    public void AddSkill(SkillData data)
    {
        if (data == null)
        {
            Debug.Log("No Skill Data!!!");
            return;
        }
        // 궁극기인지 확인 후 궁극기이면
        // 궁극기 칸이 비어있거나, 쿨타임 진행 중이지 않으면 교체
        // 궁극기 칸이 이미 차있고, 쿨타임 진행 중이면 쿨타임 중엔 교체 불가 표시
        if (data.IsUltimate)
        {
            if (_ultimateSkillSlot != null)
            {
                Debug.Log("You already have ultimate skill!!!");
                return;
            }

            GameObject go = new GameObject(data.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up;
            SkillSlot slot = go.AddComponent<SkillSlot>();
            SkillManager.Instance.SubscribeEvents(slot, -1);
            // 궁극기는 굳이 스킬 인터벌 줄 이유가?
            slot.OnActivateSkill += StartSkillInterval;
            slot.SetSkill(data);
            _ultimateSkillSlot = slot;
            return;
        }

        // 이미 스킬 아이콘 슬롯에 보유 중인 스킬인지 검사
        SkillSlot skillSlot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
        if (skillSlot != null)
        {
            Debug.Log($"Skill named {data.SkillName} already exists!!!");
            return;
        }

        GameObject skillResource = ObjectManager.Instance.PlayerSkillResourceList[data.name];
        // 패시브면 효과 적용만 시키고, 슬롯은 x
        if (skillResource.GetComponent<PassiveSkill>() != null)
        {
            PassiveSkill passive = Instantiate(skillResource).GetComponent<PassiveSkill>();
            passive.Initialize(_player.PlayerData);
        }
        // 액티브면 슬롯 만들어서 저장 및 관리
        else
        {
            GameObject go = new GameObject(data.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up;

            // 기본 공격이면 기본공격 슬롯 따로 만들어서 저장 및 관리
            if (data.name == "PlayerBasicAttack")
            {
                _basicSkillSlot = go.AddComponent<BasicSkillSlot>();
                _basicSkillSlot.SetSkill(data);
                OnShortestSkillDistanceChanged?.Invoke(data.TargetDistance);
            }

            else
            {
                // 이미 스킬 아이콘 슬롯 자리 다 찼으면 슬롯 생성 x
                if (_skillSlotCount == _player.PlayerData.UnlockedSkillSlotCount)
                {
                    Debug.Log("Slot list is already full!!!");
                    Destroy(go);
                    return;
                }

                SkillSlot slot = go.AddComponent<SkillSlot>();
                // 비어있는 인덱스 있나 찾아보고
                int idx = _activeSkillSlotList.FindIndex((slot) => slot == null);

                // 비어있는 인덱스 없는 경우(idx == -1)
                if (idx < 0)
                {
                    _activeSkillSlotList.Add(slot);
                    SkillManager.Instance.SubscribeEvents(slot, _activeSkillSlotList.Count - 1);
                    slot.OnActivateSkill += StartSkillInterval;
                }
                else
                {
                    _activeSkillSlotList[idx] = slot;
                    SkillManager.Instance.SubscribeEvents(slot, idx);
                    slot.OnActivateSkill += StartSkillInterval;
                }
                if (slot.SetSkill(data))
                {
                    _skillSlotCount++;
                }
            }
        }

    }

    public void RemoveSkill(SkillData data)
    {
        if (data == null)
        {
            Debug.Log("No Skill Data!!!");
            return;
        }
        if (data.IsUltimate)
        {
            if (_ultimateSkillSlot == null || _ultimateSkillSlot.SkillData != data)
            {
                Debug.Log($"Your ultimate skill is not {data.SkillName}");
                return;
            }
            _ultimateSkillSlot.DestroySkillSlot();
        }
        else
        {
            SkillSlot slot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
            if (slot == null)
            {
                Debug.Log("Cannot Find Skill with Name " + data.SkillName);
                return;
            }
            slot.DestroySkillSlot();
            _skillSlotCount--;
        }

    }

    public float GetShortestSkillDistance()
    {
        float min = -1;
        foreach (var slot in _activeSkillSlotList)
        {
            if (slot != null)
            {
                if (min < 0)
                {
                    min = slot.SkillData.TargetDistance;
                }
                else
                {
                    min = Mathf.Min(min, slot.SkillData.TargetDistance);
                }
            }
        }
        if (min < 0)
        {
            min = BasicSkillSlot.SkillData.TargetDistance;
        }

        OnShortestSkillDistanceChanged?.Invoke(min);
        return min;
    }

    void StartSkillInterval(float cool = default)
    {
        StartCoroutine(CoSkillInterval());
    }

    IEnumerator CoSkillInterval()
    {
        SkillManager.Instance.IsSkillInterval = true;
        yield return new WaitForSeconds(Define.SkillInterval);
        SkillManager.Instance.IsSkillInterval = false;
    }

    // 플레이어 텔레포트 시 스킬도 함께 텔포
    void OnPlayerCross()
    {
        foreach (var slot in _activeSkillSlotList)
        {
            if (slot.Skill != null && slot.Skill.gameObject.activeSelf)
            {
                slot.Skill.transform.Translate(Vector3.back * Define.TeleportDistance);
            }
        }
    }

    public void IncreaseUnlockedSkillSlotCount()
    {
        if (_skillSlotCount >= Define.MaxUnlockedSkillSlotCount)
        {
            Debug.Log("Cannot increase skill slot any more!!!");
            return;
        }
        SetSkillSlotList();
        SkillManager.Instance.UpdateEnhancedAttribute(EnvironmentManager.Instance.CurrentType);
    }

    public void DecreaseUnlockedSkillSlotCount()
    {
        if (_skillSlotCount <= 2)
        {
            Debug.Log("Cannot decrease skill slot any more!!!");
            return;
        }
        _activeSkillSlotList[_skillSlotCount - 1].DestroySkillSlot();
        SkillManager.Instance.LockIconSlots(_skillSlotCount - 1);
        SkillManager.Instance.UpdateEnhancedAttribute(EnvironmentManager.Instance.CurrentType);
        _skillSlotCount--;
    }
}