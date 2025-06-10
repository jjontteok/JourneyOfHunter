using UnityEngine;

public class BasicSkillSlot : SkillSlot
{
    public ActiveSkill Skill { get { return _skill; } }

    public override void ActivateSlotSkill()
    {
        if (IsActivatePossible)
        {
            Transform target = Util.GetNearestTarget(transform.position, _skill.SkillData.targetDistance)?.transform;
            if (target != null)
            {
                _skill.ActivateSkill(target, transform.position);

                IsActivatePossible = false;
                StartCoroutine(CoStartCoolTime());
            }
        }
    }
}
