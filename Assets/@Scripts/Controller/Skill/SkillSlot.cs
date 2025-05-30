using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

/// SkillSlot
/// Target형 스킬들을 보관 및 스킬 쿨타임 관리하는 슬롯
/// NonTarget형 스킬들은 쿨되면 알아서 발동되므로 필요없을듯(일단은)
public class SkillSlot : MonoBehaviour
{
    // 스킬 슬롯에는 액티브형 스킬만 저장
    public ActiveSkill _skill;

    PlayerController _player;
    Transform _target;
    bool _isTargetExist;
    bool _isBasicSkill;

    static bool s_isAttacking;

    // 슬롯에 등록된 스킬의 사용 가능 여부
    public bool IsActivatePossible { get; set; }

    private void Awake()
    {
        Initialize();
    }
    protected virtual void Initialize()
    {
        // 나중에 게임매니저에서 가져오든지 할 예정
        _player = FindAnyObjectByType<PlayerController>();
        IsActivatePossible = false;
    }

    // 처음 슬롯 생성 시 스킬 등록
    public void SetSkill(Skill skill)
    {
        // 맨 처음엔 사용 가능한 상태
        IsActivatePossible = true;
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
        if (_skill.SkillData.name == "PlayerBasicAttack")
        {
            _isBasicSkill = true;
        }
        else
        {
            _isBasicSkill = false;
        }
        _skill.Initialize();
        _skill.gameObject.SetActive(false);
    }

    IEnumerator CoStartCoolTime()
    {
        yield return new WaitForSeconds(_skill.SkillData.coolTime);
        IsActivatePossible = true;
        s_isAttacking = false;
    }

    private void Update()
    {
        // 기본 공격이 아닌, 스킬 슬롯인 경우
        if (!_isBasicSkill)
        {
            // 쿨타임 초기화되어 스킬 사용 가능한지, 플레이어의 마나가 충분한지 체크
            if (IsActivatePossible && _player.MP >= _skill.SkillData.MP)
            {
                // Target형 스킬인 경우
                if (_isTargetExist)
                {
                    // 가장 가까운 타겟을 탐색하고, 있으면 스킬 발동
                    _target = GetNearestTarget(_skill.SkillData.targetDistance)?.transform;
                    if (_target != null)
                    {
                        ActivateSlotSkill(_target);
                    }
                }
                // Target형 스킬이 아닌 경우
                else
                {
                    ActivateSlotSkill();
                }
                _player.MP = Mathf.Max(_player.MP - _skill.SkillData.MP, 0);
                Debug.Log($"Current Player MP: {_player.MP}");
            }
        }
        // 기본 공격 슬롯인 경우
        else
        {
            // 기본 공격 중이지 않고, 다른 스킬들이 시전 중이지 않을 때
            if (IsActivatePossible && !s_isAttacking)
            {
                ActivateSlotSkill();
            }
        }

    }

    void ActivateSlotSkill(Transform target = null)
    {
        IsActivatePossible = false;
        _skill.ActivateSkill(target, transform.position);
        StartCoroutine(CoStartCoolTime());
    }

    GameObject GetNearestTarget(float distance)
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
