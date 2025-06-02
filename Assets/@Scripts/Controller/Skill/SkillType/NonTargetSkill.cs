using UnityEngine;

public class NonTargetSkill : ActiveSkill
{
    public override void ActivateSkill(Transform target = null, Vector3 pos = default)
    {
        base.ActivateSkill(null, pos);
    }
}
