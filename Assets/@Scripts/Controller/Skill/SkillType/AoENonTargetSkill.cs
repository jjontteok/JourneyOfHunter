using UnityEngine;

public class AoENonTargetSkill : NonTargetSkill
{
    PenetrationColliderController _coll;

    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }
}
