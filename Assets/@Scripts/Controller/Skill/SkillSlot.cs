using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// SkillSlot
public class SkillSlot : MonoBehaviour
{
    // 스킬 슬롯에는 액티브형 스킬만 저장
    protected ActiveSkill _skill;
    protected PlayerController _player;

    Transform _target;
    bool _isTargetExist;

    // 스킬 슬롯 생성 시 스킬 아이콘 슬롯에 등록하는 이벤트
    public Action<SkillData> OnGenerateSlot;
    // 스킬 발동 시 스킬 아이콘 슬롯에 쿨타임 적용하는 이벤트
    public Action OnActivateSkill;
    // 스킬 제거 시 스킬 아이콘 슬롯에서 사라지게 적용하는 이벤트
    public Action OnRemoveSkill;

    // 슬롯에 등록된 스킬의 사용 가능 여부
    public bool IsActivatePossible { get; set; }

    public SkillData SkillData
    {
        get { return _skill.SkillData; }
    }

    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        // 나중에 게임매니저에서 가져오든지 할 예정
        _player = FindAnyObjectByType<PlayerController>();
        IsActivatePossible = false;
    }

    // 처음 슬롯 생성 시 스킬 등록
    public void SetSkill(SkillData data)
    {
        // 맨 처음에 약간의 딜레이 제공
        StartCoroutine(CoStartCoolTime(0.5f));

        // ObjectManager에서 skill resource를 찾아 떠나는 멀고도 험한 여정
        Skill skill = ObjectManager.Instance.PlayerSkillResourceList.FirstOrDefault((resource) =>
        resource.Value.GetComponent<Skill>().SkillData.skillName == data.skillName).Value?.GetComponent<Skill>();

        if (skill == null)
        {
            Debug.Log("Cannot Find Skill Resource named " + data.skillName);
            return;
        }

        _skill = Instantiate(skill).GetComponent<ActiveSkill>();

        // 타겟이 필요한 스킬인지 아닌지 체크
        if (_skill.SkillData.targetExistence)
        {
            _isTargetExist = true;
        }
        else
        {
            _isTargetExist = false;
        }
        _skill.Initialize();
        _skill.gameObject.SetActive(false);

        OnGenerateSlot?.Invoke(data);
    }

    protected IEnumerator CoStartCoolTime(float time = default)
    {
        if (time == default)
        {
            yield return new WaitForSeconds(_skill.SkillData.coolTime);
        }
        else
        {
            yield return new WaitForSeconds(time);
        }
        IsActivatePossible = true;
    }

    public virtual void ActivateSlotSkill()
    {
        if (IsActivatePossible && _player.MP >= _skill.SkillData.MP)
        {
            // Target형 스킬인 경우
            if (_isTargetExist)
            {
                // 가장 가까운 타겟을 탐색하고, 있으면 스킬 발동
                _target = Util.GetNearestTarget(transform.position, _skill.SkillData.targetDistance)?.transform;
                if (_target != null)
                {
                    ProcessSkill(_target);
                }
            }
            // Target형 스킬이 아닌 경우
            else
            {
                ProcessSkill();
            }
        }
    }

    // 스킬 발동 & 마나 계산 & 쿨타임 시작
    void ProcessSkill(Transform target = null)
    {
        _skill.ActivateSkill(_target, transform.position);
        _player.MP = Mathf.Max(_player.MP - _skill.SkillData.MP, 0);
        //Debug.Log($"Skill Name: {_skill.name} Current Player MP: {_player.MP}");
        IsActivatePossible = false;
        StartCoroutine(CoStartCoolTime());
        OnActivateSkill?.Invoke();
    }

    public void DestroySkillSlot()
    {
        OnRemoveSkill?.Invoke();
        Destroy(_skill.gameObject);
        Destroy(gameObject);
    }
}
