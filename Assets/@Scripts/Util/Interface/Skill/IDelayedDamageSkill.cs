using System.Collections;
using UnityEngine;

// 메테오처럼, 대상에 맞았다고 대미지 발생이 아닌,
// 일정 시간 딜레이 후 이펙트와 함께 콜라이더 활성화되며 대미지 발생하는 스킬
public interface IDelayedDamageSkill
{
    IEnumerator CoActivateDelayedCollider();
}
