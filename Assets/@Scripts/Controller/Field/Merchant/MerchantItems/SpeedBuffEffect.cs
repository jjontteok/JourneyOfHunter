using UnityEngine;

public class SpeedBuffEffect : ItemEffect
{
    public SpeedBuffEffect()
    {
        grade = Define.ItemValue.Common;
    }

    public override void ApplyEffect(Define.ConsumeTarget target)
    {
        //플레이어 속도 증가
    }
}
