using UnityEngine;

public class RigidbodyTargetSkill : TargetSkill
{
    Rigidbody _rigidbody;

    public override void Initialize()
    {
        base.Initialize();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _direction.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = _direction * _skillData.force;
        _rigidbody.linearVelocity = Vector3.zero;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.GroundTag))
        {
            ActivateConnectedSkill();
        }
        if(other.CompareTag(Define.PlayerTag))
        {
            GameObject hitEffect = Instantiate(_skillData.hitEffectPrefab);
            hitEffect.transform.position = transform.position;
        }
    }

    void ActivateConnectedSkill()
    {
        GameObject connectedSkill = Instantiate(_skillData.connectedSkillPrefab);
        connectedSkill.transform.position = transform.position;
        Skill skill = connectedSkill.GetComponent<Skill>();
        switch (skill.SkillData.skillType)
        {
            case Define.SkillType.RigidbodyTarget:

                break;
            case Define.SkillType.TransformTarget:
                break;
            case Define.SkillType.AoENonTarget:
                break;
            case Define.SkillType.DirectionNonTarget:
                break;
            case Define.SkillType.Buff:
                break;
            default:
                break;
        }
    }
}
