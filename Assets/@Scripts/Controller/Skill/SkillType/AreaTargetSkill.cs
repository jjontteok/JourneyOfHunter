using UnityEngine;

public class AreaTargetSkill : ActiveSkill, ITargetSkill, IPositioningSkill
{
    protected SkillColliderController _coll;
    protected Transform _target;
    Rigidbody _rigidbody;
    Vector3 _offset;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
        _offset = SkillData.Offset;
    }

    // target받아서 그 위치에 생성
    public override bool ActivateSkill(Vector3 pos)
    {
        if (IsTargetExist(pos, SkillData.IsPlayerSkill))
        {
            base.ActivateSkill(GetCastPosition(_target.position));
            _coll.transform.localPosition = Vector3.zero;

            if (_rigidbody)
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
            return true;
        }

        return false;
    }

    public bool IsTargetExist(Vector3 pos, bool isPlayerSkill)
    {
        _target = Util.GetNearestTarget(pos, _skillData.TargetDistance, isPlayerSkill)?.transform;
        return _target != null;
    }

    public Vector3 GetCastPosition(Vector3 pos)
    {
        if (pos.y > 0.5f)
        {
            pos.y = 0.2f;
        }
        return pos + _offset;
    }
}
