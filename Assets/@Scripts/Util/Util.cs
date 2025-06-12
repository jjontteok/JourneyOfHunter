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

    // Get angle between two normalized vectors
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
}
