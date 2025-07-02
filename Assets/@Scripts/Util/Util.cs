using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Util
{
    static public bool NullCheck<T>(T t) where T : Object
    {
        if(t == null)
        {
            Debug.Log(t.ToString() + " : Null Reference Exception");
            return true;
        }
        return false;
    }

    public static GameObject GetNearestTarget(Vector3 origin, float distance, bool isPlayerSkill = true)
    {
        Collider[] targets;
        
        if(isPlayerSkill)
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
        float height = other.GetComponent<CapsuleCollider>().height;
        height *= other.transform.lossyScale.y;
        Vector3 pos = other.transform.position;
        pos.y = height * 0.7f;
        return pos;
    }

    // Get Angle between two normalized vectors
    public static float GetAngleBetweenDirections(Vector3 from, Vector3 to)
    {
        float dot = Vector3.Dot(from, to);
        float degree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return degree;
    }

    // 콜라이더가 각도 내에 있는지 판별
    public static bool IsColliderInRange(Collider collider,Transform origin, float angle)
    {
        Vector3 toMonster = (collider.transform.position -origin.position).normalized;
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

    // 스킬 속성에 따라 현재 시간대와 날씨에 의한 대미지 증감 효과 계산
    public static float GetEnhancedDamage(SkillData data)
    {
        float damage = data.Damage;
        switch(EnvironmentManager.Instance.CurrentType)
        {
            // <최종 기획>
            // 일단 대미지 20퍼 증가
            // 아침 - 물, 빛
            // 낮 - 불, 빛
            // 저녁 - 어둠, 불
            // 밤 - 어둠, 물
            // <환경 기획>
            // 일출 - 빛 10퍼
            // 낮 - 빛 20퍼
            // 일몰 - 어둠 10퍼
            // 밤 - 어둠 20퍼
            // 비 - 물 +20퍼, 불 -20퍼
            // 폭풍? - 없애도될듯?
            // 맑음 - 불 +30퍼
            case Define.TimeOfDayType.Morning:
                if(data.SkillAttribute==Define.SkillAttribute.Water|| data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    damage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Noon:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Light)
                {
                    damage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Evening:
                if (data.SkillAttribute == Define.SkillAttribute.Fire || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    damage *= 1.2f;
                }
                break;

            case Define.TimeOfDayType.Night:
                if (data.SkillAttribute == Define.SkillAttribute.Water || data.SkillAttribute == Define.SkillAttribute.Dark)
                {
                    damage *= 1.2f;
                }
                break;

            default:
                break;
        }
        return damage;
    }
}
