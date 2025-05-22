using UnityEngine;

//타겟형 스킬에 부착될 스크립트
public class TargetSkill : ActiveSkill
{
    protected override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(target);
        //// 타겟형 스킬인 경우, 방향 설정 및 콜라이더 위치 조정
        //if (target != null)
        //{
        //    //타겟 방향으로 스킬 방향 설정
        //    Vector3 dir = (target.position - _player.transform.position).normalized;
        //    _player.Rotate(dir);

        //    //플레이어 위치에 스킬 활성화
        //    transform.localPosition = Vector3.zero;
        //    _coll.transform.localPosition = Vector3.zero;
        //    _coll.SetColliderDirection(Vector3.forward);
        //}
        //gameObject.SetActive(true);
        //// particle system인 경우
        //gameObject.GetComponent<ParticleSystem>()?.Play();

        //StartCoroutine(DeActivateSkill()); //스킬 시전 후 스킬 비활성화
    }
}
