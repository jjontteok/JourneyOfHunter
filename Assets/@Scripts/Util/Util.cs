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

    public static GameObject GetNearestTarget(Vector3 origin,float distance)
    {
        //거리 내의 monster collider 탐색
        Collider[] targets = Physics.OverlapSphere(origin, distance, 1 << LayerMask.NameToLayer(Define.MonsterTag));
        if (targets == null)
            return null;
        HashSet<Collider> neighbors = new HashSet<Collider>(targets);

        //거리 순으로 정렬하여 가장 가까운 적을 반환
        var neighbor = neighbors.OrderBy(coll => (origin - coll.transform.position).sqrMagnitude).FirstOrDefault();
        if (neighbor == null)
            return null;

        return neighbor.gameObject;
    }
}
