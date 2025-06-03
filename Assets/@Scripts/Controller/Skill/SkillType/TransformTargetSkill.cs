using System;
using UnityEngine;

public class TransformTargetSkill : ActiveSkill
{
    PenetrationColliderController _coll;
    public event Action<Vector3> OnSkillSet;

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _playerController.PlayerData.Atk, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;

        //타겟 방향으로 스킬 방향 설정
        //스킬이 땅으로 박히지 않도록 높이 맞춰주기
        _direction = (target.position + Vector3.up - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(_direction);

        OnSkillSet?.Invoke(_direction);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
        {
            _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
        }
    }
}
