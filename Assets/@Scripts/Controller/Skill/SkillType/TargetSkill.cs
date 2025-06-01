using System;
using UnityEngine;

//타겟형 스킬에 부착될 스크립트
public class TargetSkill : ActiveSkill
{
    public event Action<Vector3> OnSkillSet;

    protected Vector3 _direction;

    // 스킬의 방향 설정 추가
    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        //타겟 방향으로 스킬 방향 설정
        //스킬이 땅으로 박히지 않도록 높이 맞춰주기
        _direction = (target.position + Vector3.up - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(_direction);
        //transform.Rotate(new Vector3(0, 0, 90f));

        OnSkillSet?.Invoke(_direction);
    }
}
