using System;
using UnityEngine;

public class RotationTargetSkill : TransformTargetSkill, IRotationSkill
{
    // TransformTargetSkill + 타겟 방향으로 시전자 회전 기능
    // ex) 기본 공격
    public event Action<Vector3> OnActivateSkill;

    // 방향 설정 + 타겟 설정
    public override bool ActivateSkill(Vector3 pos)
    {
        if (base.ActivateSkill(pos))
        {
            OnActivateSkill?.Invoke(_direction);

            return true;
        }

        return false;
    }
}
