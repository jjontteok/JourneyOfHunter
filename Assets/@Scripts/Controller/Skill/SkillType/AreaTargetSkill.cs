using UnityEngine;

public class AreaTargetSkill : ActiveSkill
{
    //PenetrationColliderController _coll;
    SkillColliderController _coll;
    [SerializeField] Vector3 _offset;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        //_coll = GetComponentInChildren<PenetrationColliderController>();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, status, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    // target받아서 그 위치에 생성
    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;
        transform.position = target.position;
        transform.position += _offset;
    }
}
