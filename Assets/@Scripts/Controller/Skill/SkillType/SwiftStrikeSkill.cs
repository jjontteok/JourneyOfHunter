using System;
using System.Collections;
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
            _fixedDirection = _direction;
            OnSkillActivated?.Invoke(SkillData.durationTime);
            return true;
        }

        return false;
    }

    public override void MoveSkillCollider()
    {
        if (Vector3.Distance(transform.position, _originPos) < _skillData.targetDistance)
        {
            _playerController.transform.Translate(_fixedDirection * _skillData.speed * Time.deltaTime);
            _coll.transform.position = _playerController.transform.position;
        }
    }
}
