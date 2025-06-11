using UnityEngine;

public class AoENonTargetSkill : ActiveSkill
{
    PenetrationColliderController _coll;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, status, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }
}
