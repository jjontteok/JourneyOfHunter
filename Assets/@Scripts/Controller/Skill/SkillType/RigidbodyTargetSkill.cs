using UnityEngine;

public class RigidbodyTargetSkill : ActiveSkill
{
    [SerializeField] Vector3 _offset = Vector3.up * 5f;
    CrashColliderController _coll;
    Rigidbody _rigidbody;

    public override void Initialize()
    {
        base.Initialize();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _coll = GetComponentInChildren<CrashColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _playerController.PlayerData.Atk, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab, _skillData.angle);
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        pos += _offset;
        base.ActivateSkill(target, pos);
        _coll.gameObject.transform.localPosition = Vector3.zero;

        _rigidbody.linearVelocity = Vector3.zero;
        Vector3 difference = target.position - pos;
        Vector3 dir = difference.normalized;
        dir.y = _rigidbody.linearVelocity.y;
        //dir.y /= 2f;
        //_skillData.force = difference.magnitude + 10;
        _rigidbody.linearVelocity = dir * (difference.magnitude + 10);
        //_rigidbody.AddForce(dir * (difference.magnitude * 10));
    }

}
