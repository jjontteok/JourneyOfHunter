using UnityEngine;

// 충돌 시 사라져야 하는 스킬의 콜라이더
public class CrashColliderController : SkillColliderController
{
    protected override void InstantiateHitEffect(Collider other)
    {
        base.InstantiateHitEffect(other);
        // CrashCollider는 충돌 발생 시 사라져야 함
        transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag) || other.CompareTag(Define.FieldObjectTag))
        {
            ProcessTrigger(other);
        }

        // Activate when collide with Ground
        if (other.CompareTag(Define.GroundTag))
        {
            ActivateConnectedSkill();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
