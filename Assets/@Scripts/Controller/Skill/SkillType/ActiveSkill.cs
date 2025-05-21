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
        SkillColliderController[] colls = GetComponentsInChildren<SkillColliderController>();
        foreach (var coll in colls)
            Debug.Log(coll.name);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage);
    }
}
