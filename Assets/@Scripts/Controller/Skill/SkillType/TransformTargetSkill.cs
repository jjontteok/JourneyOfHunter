using System;
using UnityEngine;

public class TransformTargetSkill : ActiveSkill, ITargetSkill, IMovingSkill
{
    SkillColliderController _coll;

    Transform _target;
    float _speed;
    Vector3 _direction;

    public Transform Target => _target;

    public float Speed { get => _speed; set => _speed = value; }

    public Vector3 Direction { get => _direction; }

    public event Action<Vector3> OnSkillSet;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, status, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab, _skillData.angle);
    }

    // 방향 설정 + 타겟 설정
    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;

        SetDirection();

        OnSkillSet?.Invoke(_direction);
    }

    private void Update()
    {
        MoveSkillCollider();
    }

    public void SetTarget()
    {
        _target = Util.GetNearestTarget(transform.position, _skillData.targetDistance)?.transform;
    }

    public void MoveSkillCollider()
    {
        if (Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
        {
            _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
        }
    }

    public void SetDirection()
    {
        //타겟 방향으로 스킬 방향 설정
        //스킬이 땅으로 박히지 않도록 높이 맞춰주기
        _direction = (_target.position + Vector3.up - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}
