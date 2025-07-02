//using UnityEngine;

//public class RigidbodyTargetSkill : ActiveSkill, ITargetSkill
//{
//    [SerializeField] Vector3 _offset = Vector3.up * 5f;
//    CrashColliderController _coll;
//    Rigidbody _rigidbody;
//    Transform _target;

//    public override void Initialize(Status status)
//    {
//        base.Initialize(status);
//        _rigidbody = GetComponentInChildren<Rigidbody>();
//        _coll = GetComponentInChildren<CrashColliderController>();
//        _coll.SetColliderInfo(_skillData.Damage, status, _skillData.ConnectedSkillPrefab, _skillData.HitEffectPrefab, _skillData.Angle);
//    }

//    public override bool ActivateSkill(Vector3 pos)
//    {
//        if(IsTargetExist(pos, SkillData.IsPlayerSkill))
//        {
//            base.ActivateSkill(_target.position + _offset);
//            _coll.gameObject.transform.localPosition = Vector3.zero;

//            _rigidbody.linearVelocity = Vector3.zero;
//            Vector3 difference = _target.position - pos;
//            Vector3 dir = difference.normalized;
//            dir.y = _rigidbody.linearVelocity.y;
//            _rigidbody.linearVelocity = dir * (difference.magnitude + 10);

//            return true;
//        }

//        return false;
//    }

//    public bool IsTargetExist(Vector3 pos, bool IsPlayerSkill)
//    {
//        _target = Util.GetNearestTarget(pos, _skillData.TargetDistance, IsPlayerSkill)?.transform;
//        return _target != null;
//    }
//}
