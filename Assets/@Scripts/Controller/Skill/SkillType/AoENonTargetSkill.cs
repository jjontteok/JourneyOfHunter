using UnityEngine;

public class AoENonTargetSkill : ActiveSkill
{
    PenetrationColliderController _coll;

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _playerController.PlayerData.Atk, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }
}
