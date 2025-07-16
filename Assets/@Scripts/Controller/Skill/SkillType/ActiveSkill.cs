using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActiveSkill : Skill
{
    protected AudioSource _skillSound;

    // 스킬 오브젝트 활성화(default 포지션) + 시전 끝나면 비활성화 코루틴
    public override bool ActivateSkill(Vector3 pos)
    {
        //시전 위치에 스킬 활성화
        //스킬 오브젝트 자체는 슬롯의 자식이 아니므로 position 필요
        gameObject.SetActive(true);
        transform.position = pos;

        if (_skillSound != null)
        {
            _skillSound.Play();
        }

        //스킬 시전 후 스킬 비활성화
        StartCoroutine(DeActivateSkill());
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _skillSound = GetComponent<AudioSource>();
    }

    protected virtual IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}
