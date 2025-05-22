using UnityEngine;

public class IceStorm : NonTargetSkill, SpellMotion
{
    public void SpellAttack()
    {
        _animator.SetTrigger(Define.Spell);
        //_animator.SetBool(Define.IsAttacking, true);
    }

    public override void ActivateSkill(Transform target)
    {
        // 공격 모션 중일 경우 모션은 생략
        if (!_animator.GetBool(Define.IsAttacking))
        {
            SpellAttack();
        }
        base.ActivateSkill(target);
    }
}
