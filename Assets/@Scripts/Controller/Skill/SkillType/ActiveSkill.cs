using System.Collections;
using UnityEngine;

public class ActiveSkill : Skill
{
    // 스킬 오브젝트 활성화(default 포지션) + 시전 끝나면 비활성화 코루틴
    public override bool ActivateSkill(Vector3 pos)
    {
        //시전 위치에 스킬 활성화
        //스킬 오브젝트 자체는 슬롯의 자식이 아니므로 position 필요
        gameObject.SetActive(true);
        transform.position = pos;

        //스킬 시전 후 스킬 비활성화
        StartCoroutine(DeActivateSkill());
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
    }

    protected virtual IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}
