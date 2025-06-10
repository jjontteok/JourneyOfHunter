using UnityEngine;

public class AreaTargetSkill : ActiveSkill, ITargetSkill
{
    //PenetrationColliderController _coll;
    SkillColliderController _coll;
    Transform _target;
    [SerializeField] Vector3 _offset;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        //_coll = GetComponentInChildren<PenetrationColliderController>();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, status, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    // target받아서 그 위치에 생성
    public override bool ActivateSkill(Vector3 pos)
    {
        if(IsTargetExist(pos))
        {
            base.ActivateSkill(_target.position + _offset);
            _coll.transform.localPosition = Vector3.zero;

            return true;
        }
        
        return false;
    }

    public bool IsTargetExist(Vector3 pos)
    {
        _target = Util.GetNearestTarget(pos, _skillData.targetDistance)?.transform;
        return _target != null;
    }
}
