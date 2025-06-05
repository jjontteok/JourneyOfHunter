using System.Collections.Generic;
using UnityEngine;

// 충돌 시 관통 or 유지되어야 하는 스킬의 콜라이더 - TransformTargetSkill, AoENonTargetSkill
public class PenetrationColliderController : SkillColliderController
{
    HashSet<GameObject> _damagedObjects = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // 스킬 시전 후 대미지 중복 적용 방지
        if(!_damagedObjects.Contains(other.gameObject))
        {
            // angle 값이 없거나, 있는데 콜라이더가 각도 내에 있을 경우 대미지
            if (_angle == 0f || (_angle > 0 && IsColliderInRange(other)))
            {
                if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
                {
                    _damagedObjects.Add(other.gameObject);
                    ActivateConnectedSkill();
                    ProcessTrigger(other);
                }
                    
            }
        }
    }

    // 콜라이더가 부채꼴 내에 있는지 판별
    bool IsColliderInRange(Collider collider)
    {
        Vector3 toMonster = (collider.transform.position - transform.position).normalized;
        float degree = GetAngleBetweenDirections(toMonster, transform.forward);
        if (degree <= _angle / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }    

    // Get angle between two normalized vectors
    float GetAngleBetweenDirections(Vector3 from, Vector3 to)
    {
        float dot = Vector3.Dot(from, to);
        float degree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return degree;
    }

    private void OnDisable()
    {
        _damagedObjects.Clear();
    }
}
