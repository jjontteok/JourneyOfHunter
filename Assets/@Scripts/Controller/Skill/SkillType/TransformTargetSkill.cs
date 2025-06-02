using UnityEngine;

public class TransformTargetSkill : TargetSkill
{
    PenetrationColliderController _coll;

    void Start()
    {
        Initialize();
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _playerController.PlayerData.Atk, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
        {
            _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
        }
    }
}
