using System;
using UnityEngine;

public class SwiftStrikeSkill : RotationTargetSkill, ICharacterMovingSkill
{
    Vector3 _originPos;
    Vector3 _fixedDirection;

    public event Action<float> OnSkillActivated;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        if(base.ActivateSkill(pos))
        {
            _originPos = transform.position;
            Vector3 tmp = _direction;
            tmp.y = 0f;
            _fixedDirection = tmp;
            OnSkillActivated?.Invoke(SkillData.durationTime);
            return true;
        }

        return false;
    }

    public override void MoveSkillCollider()
    {
        if (Vector3.Distance(_coll.transform.position, _originPos) < _skillData.targetDistance)
        {
            _playerController.transform.Translate(_fixedDirection * _skillData.speed * Time.deltaTime, Space.World);
            _coll.transform.position = _playerController.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + _direction * _skillData.targetDistance + Vector3.up * 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _fixedDirection * _skillData.targetDistance);
    }
}
