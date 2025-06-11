using UnityEngine;

// PenetrationColliderController + 각도 설정 기능 스킬
public class AngularColliderController : PenetrationColliderController, IAngleCollider
{
    public bool CheckInRange(Collider collider, float angle)
    {
        return Util.IsColliderInRange(collider, transform.parent, angle);
    }

    protected override void ProcessTrigger(Collider other)
    {
        if (CheckInRange(other, _skillData.angle))
        {
            base.ProcessTrigger(other);
        }
    }
}
