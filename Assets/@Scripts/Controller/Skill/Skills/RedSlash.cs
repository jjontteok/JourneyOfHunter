using UnityEngine;

public class RedSlash : TargetSkill, SwordMotion
{
    Vector3 _euler = new Vector3(0, 0, 90f);

    public void SwordAttack()
    {
        _animator.SetTrigger(Define.Attack);
        //_animator.SetBool(Define.IsAttacking, true);
    }

    public override void ActivateSkill(Transform target)
    {
        // 공격 모션 중일 경우 모션은 생략
        if (!_animator.GetBool(Define.IsAttacking))
        {
            SwordAttack();
        }
        // 검기의 경우, 90도 회전해서 눕혀진 상태로 발사
        transform.localRotation = Quaternion.Euler(_euler);
        base.ActivateSkill(target);
    }
}
