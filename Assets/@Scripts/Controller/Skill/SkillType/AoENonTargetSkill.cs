using UnityEngine;

public class AoENonTargetSkill : ActiveSkill
{
    SkillColliderController _coll;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        base.ActivateSkill(pos);
        RaycastHit hit;
        Physics.Raycast(pos, Vector3.down, out hit, 5f, LayerMask.GetMask(Define.GroundTag));
        Vector3 position = hit.point;
        position.y += 0.3f;
        transform.position = position;
        return true;
    }
}
