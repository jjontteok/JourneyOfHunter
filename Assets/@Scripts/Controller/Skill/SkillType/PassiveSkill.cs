using System.Collections;
using UnityEngine;

public class PassiveSkill : Skill, IStatusChangeSkill
{
    // 씬 시작 시, 패시브 스킬은 플레이어 런타임 데이터에 패시브 적용만 함
    public override bool ActivateSkill(Vector3 pos = default)
    {
        StatusChange(true);
        return true;
    }

    public override void Initialize(Status status)
    {
        _player = PlayerManager.Instance.Player;
        ActivateSkill();
    }

    // 패시브 적용 시 true, 적용 사항 롤백 시 false
    public void StatusChange(bool flag)
    {
        float coeff = SkillData.buffAmount;

        if (!flag)
        {
            coeff *= -1;
        }

        _player.OnOffStatusUpgrade(SkillData.buffStatus, coeff);
    }
}
