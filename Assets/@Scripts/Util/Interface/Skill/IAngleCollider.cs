using UnityEngine;

// 일정 각도 내에 시전되는 기능
public interface IAngleCollider
{
    bool CheckInRange(Collider collider, float angle);
}
