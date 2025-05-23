using UnityEngine;

public class AoENonTargetSkill : NonTargetSkill
{
    SkillColliderController _coll;
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage, _skillData.targetDistance, _skillData.castingTime, _skillData.hitEffectPrefab);
    }
}
