using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pair<T, U>
{
    public Pair(T first, U second)
    {
        First = first;
        Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public class Util
{
    static public bool NullCheck<T>(T t) where T : Object
    {
        if (t == null)
        {
            Debug.Log(t.ToString() + " : Null Reference Exception");
            return true;
        }
        return false;
    }

    public static GameObject GetNearestTarget(Vector3 origin, float distance, bool isPlayerSkill = true)
    {
        if (FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
        {
            GameObject go = GameObject.FindGameObjectWithTag(Define.FieldObjectTag);
            if (go.GetComponent<Animator>().GetBool(Define.Open) || Vector3.Distance(origin, go.transform.position) > distance)
            {
                return null;
            }
            return go;
        }

        Collider[] targets;

        if (isPlayerSkill)
        {
            //거리 내의 monster collider 탐색
            targets = Physics.OverlapSphere(origin, distance, 1 << LayerMask.NameToLayer(Define.MonsterTag));
        }
        // 몬스터 스킬이면 플레이어 찾는 거겠지 뭐
        else
        {
            targets = Physics.OverlapSphere(origin, distance, 1 << LayerMask.NameToLayer(Define.PlayerTag));
        }

        if (targets == null)
            return null;

        HashSet<Collider> neighbors = new HashSet<Collider>(targets);

        //거리 순으로 정렬하여 가장 가까운 적을 반환
        var neighbor = neighbors.OrderBy(coll => (origin - coll.transform.position).sqrMagnitude).FirstOrDefault();
        if (neighbor == null)
            return null;

        return neighbor.gameObject;
    }

    public static Vector3 GetDamageTextPosition(Collider other)
    {
        return other.transform.position + new Vector3(0f, other.bounds.center.y * 2, 0f);
    }

    public static Vector3 GetEffectPosition(Collider other)
    {
        // 캐릭터들의 콜라이더는 capsule로 고정
        float height;
        if (other is CapsuleCollider)
        {
            height = other.GetComponent<CapsuleCollider>().height;
        }
        else if (other is BoxCollider)
        {
            height = other.GetComponent<BoxCollider>().size.y;
        }
        else
        {
            // 땅속에 박아버리기
            return Vector3.down * -5;
        }
        height *= other.transform.lossyScale.y;
        Vector3 pos = other.transform.position;
        pos.y = (float)height * 0.7f;
        return pos;
    }

    // 두 벡터 사이의 각도 계산
    public static float GetAngleBetweenDirections(Vector3 from, Vector3 to)
    {
        float dot = Vector3.Dot(from, to);
        float degree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return degree;
    }

    // 콜라이더가 각도 내에 있는지 판별
    public static bool IsColliderInRange(Collider collider, Transform origin, float angle)
    {
        Vector3 toMonster = (collider.transform.position - origin.position).normalized;
        float degree = GetAngleBetweenDirections(toMonster, origin.forward);
        if (degree <= angle / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 스킬 속성에 따라 현재 시간대에 의한 대미지 증감 효과 계산
    public static float GetEnhancedDamage(float damage, SkillData data)
    {
        float newDamage = damage;
        //Debug.Log($"Current Damage: {newDamage}, Current DayType: {EnvironmentManager.Instance.CurrentType}, Current SkillType: {data.SkillAttribute}");
        switch (EnvironmentManager.Instance.CurrentType)
        {
            // <최종 기획>
            // 일단 대미지 20퍼 증가
            // 아침 - 물, 빛
            // 낮 - 불, 빛
            // 저녁 - 어둠, 불
            // 밤 - 어둠, 물
            case Define.TimeOfDayType.Morning:
                if (data.SkillAttribute == Define.SkillAttribute.Water || data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    newDamage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Noon:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    newDamage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Evening:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    newDamage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Night:
                if (data.SkillAttribute == Define.SkillAttribute.Water || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    newDamage *= 1.2f;
                }
                break;

            default:
                break;
        }
        //Debug.Log("Enhanced damage: " + newDamage);
        return newDamage;
    }

    // 스킬 속성에 따라 현재 시간대에 의한 쿨타임 감소 효과 수치 계산
    public static float GetCoolTimeDecreaseByDayType(SkillData data)
    {
        Debug.Log($"Current Day Type: {EnvironmentManager.Instance.CurrentType}, Current Skill Attribut: {data.SkillAttribute}");
        float reduction = 0f;
        switch (EnvironmentManager.Instance.CurrentType)
        {
            // <최종 기획>
            // 일단 대미지 20퍼 증가
            // 아침 - 물, 빛
            // 낮 - 불, 빛
            // 저녁 - 어둠, 불
            // 밤 - 어둠, 물
            case Define.TimeOfDayType.Morning:
                if (data.SkillAttribute == Define.SkillAttribute.Water || data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    reduction = -20;
                }
                break;

            case Define.TimeOfDayType.Noon:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    reduction = -20;
                }
                break;

            case Define.TimeOfDayType.Evening:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    reduction = -20;
                }
                break;

            case Define.TimeOfDayType.Night:
                if (data.SkillAttribute == Define.SkillAttribute.Water || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    reduction = -20;
                }
                break;

            default:
                break;
        }
        return reduction;
    }
}
