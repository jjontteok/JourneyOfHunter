using UnityEngine;

public interface IAngleCollider
{
    bool CheckInRange(Collider collider, float angle);
}
