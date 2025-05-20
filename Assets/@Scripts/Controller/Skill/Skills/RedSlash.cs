using UnityEngine;

public class RedSlash : TargetSkill, SwordMotion
{
    public void SwordAttack()
    {
        animator.SetTrigger(Define.Attack);
        animator.SetBool(Define.IsAttacking, true);
    }

    protected override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(target);
        SwordAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.MonsterTag))
        {
            // Monster Hit Effect
        }
    }

}
