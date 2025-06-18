using UnityEngine;

public class BasicSkillSlot : SkillSlot
{
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
