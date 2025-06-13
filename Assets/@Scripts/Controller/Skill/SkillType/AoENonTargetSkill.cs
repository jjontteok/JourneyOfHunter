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
}
