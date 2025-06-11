using UnityEngine;

public class BasicSkillSlot : SkillSlot
{
    public ActiveSkill Skill { get { return _skill; } }

    public override void ActivateSlotSkill()
    {
        if (IsActivatePossible)
        {
            if(_skill.ActivateSkill(transform.position))
            {
                IsActivatePossible = false;
                StartCoroutine(CoStartCoolTime());
                OnActivateSkill?.Invoke();
            }
        }
    }
}
