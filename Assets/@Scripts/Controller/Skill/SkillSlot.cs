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
    public void SetSkill(Skill skill)
    {
        // 맨 처음에 약간의 딜레이 제공
        StartCoroutine(CoStartCoolTime(0.5f));

        _skill = Instantiate(skill).GetComponent<ActiveSkill>();

        // 타겟이 필요한 스킬인지 아닌지 체크
        if (_skill.SkillData.skillType == Define.SkillType.RigidbodyTarget || _skill.SkillData.skillType == Define.SkillType.TransformTarget)
        {
            _isTargetExist = true;
        }
        else
        {
            _isTargetExist = false;
        }
        _skill.Initialize();
        _skill.gameObject.SetActive(false);
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

    //private void Update()
    //{
    //    // 쿨타임 초기화되어 스킬 사용 가능한지, 플레이어의 마나가 충분한지 체크
    //    if (IsActivatePossible && _player.MP >= _skill.SkillData.MP)
    //    {
    //        // Target형 스킬인 경우
    //        if (_isTargetExist)
    //        {
    //            // 가장 가까운 타겟을 탐색하고, 있으면 스킬 발동
    //            _target = GetNearestTarget(_skill.SkillData.targetDistance)?.transform;
    //            if (_target != null)
    //            {
    //                ActivateSlotSkill(_target);
    //            }
    //        }
    //        // Target형 스킬이 아닌 경우
    //        else
    //        {
    //            ActivateSlotSkill();
    //        }
    //        _player.MP = Mathf.Max(_player.MP - _skill.SkillData.MP, 0);
    //        Debug.Log($"Skill Name: {_skill.name}Current Player MP: {_player.MP}");
    //    }
    //}

    public virtual void ActivateSlotSkill()
    {
        if (IsActivatePossible && _player.MP >= _skill.SkillData.MP)
        {
            // Target형 스킬인 경우
            if (_isTargetExist)
            {
                // 가장 가까운 타겟을 탐색하고, 있으면 스킬 발동
                _target = GetNearestTarget(_skill.SkillData.targetDistance)?.transform;
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
    }

    protected GameObject GetNearestTarget(float distance)
    {
        if (_player == null)
            _player = FindAnyObjectByType<PlayerController>();
        //거리 내의 monster collider 탐색
        Collider[] targets = Physics.OverlapSphere(_player.transform.position, distance, 1 << LayerMask.NameToLayer(Define.MonsterTag));
        if (targets == null)
            return null;
        HashSet<Collider> neighbors = new HashSet<Collider>(targets);

        //거리 순으로 정렬하여 가장 가까운 적을 반환
        var neighbor = neighbors.OrderBy(coll => (_player.transform.position - coll.transform.position).sqrMagnitude).FirstOrDefault();
        if (neighbor == null)
            return null;

        return neighbor.gameObject;
    }
}
