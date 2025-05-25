using UnityEngine;

public class RigidbodyTargetSkill : TargetSkill
{
    MonsterSkillColliderController _coll;
    Rigidbody _rigidbody;

    public override void Initialize()
    {
        base.Initialize();
        _rigidbody = GetComponent<Rigidbody>();
        _coll = GetComponentInChildren<MonsterSkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.gameObject.transform.localPosition = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = dir * _skillData.force;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag(Define.GroundTag))
    //    {
    //        ActivateConnectedSkill();
    //    }
    //    if(other.CompareTag(Define.PlayerTag))
    //    {
    //        GameObject hitEffect = Instantiate(_skillData.hitEffectPrefab);
    //        hitEffect.transform.position = transform.position;
    //    }
    //}

    //void ActivateConnectedSkill()
    //{
    //    GameObject connectedSkill = Instantiate(_skillData.connectedSkillPrefab);
    //    connectedSkill.transform.position = transform.position;
    //    Skill skill = connectedSkill.GetComponent<Skill>();
    //    switch (skill.SkillData.skillType)
    //    {
    //        case Define.SkillType.RigidbodyTarget:

    //            break;
    //        case Define.SkillType.TransformTarget:
    //            break;
    //        case Define.SkillType.AoENonTarget:
    //            break;
    //        case Define.SkillType.DirectionNonTarget:
    //            break;
    //        case Define.SkillType.Buff:
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
