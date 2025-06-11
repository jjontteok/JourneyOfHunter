using UnityEngine;

public class TransformTargetSkill : ActiveSkill, ITargetSkill, IMovingSkill, IDirectionSkill
{
    SkillColliderController _coll;
    Transform _target;
    protected Vector3 _direction;

    public Vector3 Direction { get => _direction; }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    // 방향 설정 + 타겟 설정
    public override bool ActivateSkill(Vector3 pos)
    {
        if (IsTargetExist(pos, SkillData.isPlayerSkill))
        {
            base.ActivateSkill(pos);
            _coll.transform.localPosition = Vector3.zero;
            SetDirection();

            return true;
        }

        return false;
    }

    private void Update()
    {
        MoveSkillCollider();
    }

    public virtual bool IsTargetExist(Vector3 pos, bool isPlayerSkill)
    {
        _target = Util.GetNearestTarget(pos, _skillData.targetDistance, isPlayerSkill)?.transform;
        return _target != null;
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
