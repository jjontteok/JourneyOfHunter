using UnityEngine;

public class DirectionNonTargetSkill : ActiveSkill
{
    SkillColliderController _coll;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    // 스킬 발동 순간, 그 앞의 범위 내에 있는 적들에게 대미지
    public override bool ActivateSkill(Vector3 pos)
    {
        base.ActivateSkill(pos);

        // 현재 플레이어가 바라보는 방향 == 스킬 발동 방향
        // 플레이어 객체를 받아오는 방법 강구 필요        
        transform.rotation = _player.transform.rotation;

        return true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _skillData.TargetDistance);
    //    Vector3 angle1 = new Vector3(Mathf.Sin((-_skillData.Angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, (Mathf.Cos((-_skillData.Angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad)));
    //    Vector3 angle2 = new Vector3(Mathf.Sin((_skillData.Angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, (Mathf.Cos((_skillData.Angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad)));
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position, transform.position + angle1 * _skillData.TargetDistance);
    //    Gizmos.DrawLine(transform.position, transform.position + angle2 * _skillData.TargetDistance);
    //}
}
