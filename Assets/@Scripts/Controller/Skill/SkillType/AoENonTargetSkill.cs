using UnityEngine;

public class AoENonTargetSkill : ActiveSkill, ICheckActivation
{
    SkillColliderController _coll;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        // 거리 내 타겟이 있어야 발동하게 바꿈
        if (IsActivatePossible(pos))
        {
            base.ActivateSkill(pos);
            Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 5f, LayerMask.GetMask(Define.GroundTag));
            Vector3 position = hit.point;
            position.y += 0.3f;
            transform.position = position;
            return true;
        }
        return false;
    }

    public bool IsActivatePossible(Vector3 pos)
    {
        // 수동 모드일 땐 타겟 유무 상관없이 그냥 발사
        if (SkillData.IsPlayerSkill && !PlayerManager.Instance.IsAuto)
            return true;
        return FieldManager.Instance.CurrentEventType == Define.JourneyType.Dungeon;
    }
}
