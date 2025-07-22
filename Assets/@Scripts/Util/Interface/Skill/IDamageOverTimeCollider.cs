using UnityEngine;

// 도트딜 기능
public interface IDamageOverTimeCollider
{
    void TickDamage(Collider other);
}
