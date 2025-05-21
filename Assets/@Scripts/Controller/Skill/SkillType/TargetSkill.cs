using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//타겟형 스킬에 부착될 스크립트
public class TargetSkill : ActiveSkill
{
    protected override void ActivateSkill(Transform target)
    {

        //범위 내 가장 가까운 적을 찾는다
        //Transform target = GetNearestTarget(_skillData.targetDistance)?.transform;

        if (target != null)
        {
            //타겟 방향으로 스킬 방향 설정
            Vector3 dir = (target.position - _player.transform.position).normalized;

            //플레이어 위치에 스킬 생성
            //transform.position = _player.transform.position;
            //transform.rotation = Quaternion.LookRotation(dir);
            transform.localPosition = Vector3.zero;
            _player.Rotate(dir);
            gameObject.SetActive(true);
            // particle system인 경우
            gameObject.GetComponent<ParticleSystem>()?.Play();
            StartCoroutine(DeActivateSkill()); //스킬 시전 후 스킬 비활성화
        }

        //StartCoroutine(DeActivateSkill());
    }

    GameObject GetNearestTarget(float distance)
    {
        //거리 내의 monster collider 탐색
        Collider[] targets = Physics.OverlapSphere(_player.transform.position, distance, LayerMask.NameToLayer(Define.MonsterTag));
        if (targets == null)
            return null;
        HashSet<Collider> neighbors = new HashSet<Collider>(targets);

        //거리 순으로 정렬하여 가장 가까운 적을 반환
        var neighbor = neighbors.OrderBy(coll => (_player.transform.position - coll.transform.position).sqrMagnitude).FirstOrDefault();
        if (neighbor == null)
            return null;

        return neighbor.gameObject;
    }
}
