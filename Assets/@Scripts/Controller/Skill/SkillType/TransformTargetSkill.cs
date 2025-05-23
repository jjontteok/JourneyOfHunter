using UnityEngine;

public class TransformTargetSkill : TargetSkill
{
    SkillColliderController _coll;

    void Start()
    {
        Initialize();
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;
        _coll.SetColliderDirection(Vector3.forward);
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage, _skillData.targetDistance, _skillData.castingTime, _skillData.hitEffectPrefab);
    }
}
