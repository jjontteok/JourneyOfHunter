using UnityEngine;

public class ActiveSkill : Skill
{
    protected Animator _animator;
    protected SkillColliderController _coll;

    protected override void ActivateSkill(Transform target) { }

    public override void Initialize(SkillData data)
    {
        base.Initialize(data);
        _animator = _player.GetComponent<Animator>();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage);
    }
}
