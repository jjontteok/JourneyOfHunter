using System;
using UnityEngine;
using extension;

// * 공격 범위 컨트롤러 스크립트
public class AttackRangeController : MonoBehaviour
{
    public Action OnAttack;
    public Action OffAttack;

    private CapsuleCollider _attackRange;
    private Animator _animator;

    // * 초기화 메서드
    //- 위치 고정 및 콜라이더 생성, 설정
    public void Initialize(float attackRange)
    {
        transform.position = transform.parent.position;
        _animator = GetComponentInParent<Animator>();

        _attackRange = gameObject.GetOrAddComponent<CapsuleCollider>();
        _attackRange.isTrigger = true;
        _attackRange.radius = attackRange;
        _attackRange.center += Vector3.up;
    }

    // 플레이어 감지 시 공격 이벤트 발생
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"충돌 발생: {other.Name}, 태그: {other.tag}, 기대 태그: {Define.PlayerTag}");
        if (other.CompareTag(Define.PlayerTag) && other.GetComponent<Animator>().GetInteger(Define.DieType) == 0 && !_animator.GetBool(Define.IsAttacking))
        {
            //Debug.Log("충돌");
            OnAttack.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Define.PlayerTag))
        {
            OffAttack.Invoke();
        }
    }
}
