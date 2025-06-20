using System.Collections;
using UnityEngine;

public class PassiveSkill : Skill, IStatusChangeSkill
{
    // 씬 시작 시, 패시브 스킬은 플레이어 런타임 데이터에 패시브 적용만 함
    public override bool ActivateSkill(Vector3 pos = default)
    {
        StatusChange(true, false);
        return true;
    }

    public override void Initialize(Status status)
    {
        _player = PlayerManager.Instance.Player;
        ActivateSkill();
    }

    public void StatusChange(bool flag, bool isMultiply)
    {
        float coeff = SkillData.buffAmount;
        // 감소 & 합연산
        if (!flag && !isMultiply)
        {
            coeff *= -1;
        }
        // 곱연산
        else if (isMultiply)
        {
            coeff = 1 + coeff / 100;
            // 감소일 경우 역수
            if (!flag)
            {
                coeff = 1 / coeff;
            }
        }

        _player.OnOffStatusUpgrade(SkillData.buffStatus, coeff, isMultiply);
    }
}
