using UnityEngine;

public class BasicSkillSlot : SkillSlot
{
    public ActiveSkill Skill { get { return _skill; } }

    public override void ActivateSlotSkill()
    {
        if (IsActivatePossible)
        {
            //TransformTarget으로 바꿀 경우
            if (_skill.SkillData.skillType == Define.SkillType.TransformTarget)
            {
                Transform target = GetNearestTarget(_skill.SkillData.targetDistance)?.transform;
                if (target != null)
                {
                    _skill.ActivateSkill(target, transform.position);

                    Debug.Log("기본 공격 발동");
                    IsActivatePossible = false;
                    StartCoroutine(CoStartCoolTime());
                }
            }

            //기존대로 DirectionNonTarget일 경우
            else if (_skill.SkillData.skillType == Define.SkillType.DirectionNonTarget)
            {
                _skill.ActivateSkill(null, transform.position);

                Debug.Log("기본 공격 발동");
                IsActivatePossible = false;
                StartCoroutine(CoStartCoolTime());
            }
        }
    }

    bool IsTargetInRange(Transform target)
    {
        if (target == null)
            return false;

        Vector3 toTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(toTarget, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (angle <= _skill.SkillData.angle / 2)
            return true;
        else
            return false;
    }
}
