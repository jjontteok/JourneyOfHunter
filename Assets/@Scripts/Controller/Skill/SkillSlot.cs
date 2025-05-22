using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// SkillSlot
/// Target형 스킬들을 보관 및 스킬 쿨타임 관리하는 슬롯
/// NonTarget형 스킬들은 쿨되면 알아서 발동되므로 필요없을듯(일단은)
public class SkillSlot : MonoBehaviour
{
    SkillData _skillData;
    ActiveSkill _currentSkill;
    PlayerController _player;
    Transform _target;
    bool _isTargetExist;

    // 슬롯에 등록된 스킬의 사용 가능 여부
    public bool IsActivatePossible { get; set; }

    private void Awake()
    {
        // 나중에 게임매니저에서 가져오든지 할 예정
        _player = FindAnyObjectByType<PlayerController>();
        IsActivatePossible = false;
    }

    // 처음 슬롯 생성 시 스킬 등록
    public void SetSkill(SkillData skillData)
    {
        // 맨 처음엔 사용 가능한 상태
        IsActivatePossible = true;
        _skillData = skillData;
        _currentSkill = Instantiate(_skillData.skillPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ActiveSkill>();
        // 타겟이 필요한 스킬인지 아닌지 체크
        TargetSkill skill = _currentSkill.GetComponent<TargetSkill>();
        if (skill != null)
        {
            _isTargetExist = true;
        }
        else
        {
            _isTargetExist = false;
        }
        _currentSkill.Initialize(skillData);
        _currentSkill.gameObject.SetActive(false);
        // 스킬 오브젝트 생성되면 활성화
        IsActivatePossible = true;
    }

    public IEnumerator CoStartCoolTime()
    {
        yield return new WaitForSeconds(_skillData.coolTime);
        IsActivatePossible = true;
    }

    private void Update()
    {
        // 쿨타임 초기화되어 스킬 사용 가능한 경우
        if (IsActivatePossible)
        {
            // Target형 스킬인 경우
            if (_isTargetExist)
            {
                // 가장 가까운 타겟을 탐색하고, 있으면 스킬 발동
                _target = GetNearestTarget(_skillData.targetDistance)?.transform;
                if (_target != null)
                {
                    IsActivatePossible = false;
                    _currentSkill.StartAttack(_target);
                    StartCoroutine(CoStartCoolTime());
                }
            }
            // Target형 스킬이 아닌 경우
            else
            {
                IsActivatePossible = false;
                _currentSkill.StartAttack(null);
                StartCoroutine(CoStartCoolTime());
            }
        }
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
