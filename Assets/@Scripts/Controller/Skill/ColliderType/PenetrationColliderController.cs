using System.Collections.Generic;
using UnityEngine;

// 충돌해도 사라지지 않는 스킬의 콜라이더
public class PenetrationColliderController : SkillColliderController
{
    HashSet<GameObject> _damagedObjects = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
        {
            // 스킬 시전 후 대미지 중복 적용 방지
            if (!_damagedObjects.Contains(other.gameObject))
            {
                ProcessTrigger(other);
            }
        }
    }

    protected override void ProcessTrigger(Collider other)
    {
        _damagedObjects.Add(other.gameObject);
        ActivateConnectedSkill();
        base.ProcessTrigger(other);
    }

    private void OnDisable()
    {
        _damagedObjects.Clear();
    }
}
