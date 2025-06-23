using System.Collections;
using UnityEngine;

public class BuffSkill : ActiveSkill, IStatusChangeSkill
{
    bool _isCoroutineRunning = false;

    public override bool ActivateSkill(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        StatusChange(true);

        StartCoroutine(DeActivateSkill());
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
    }

    private void Update()
    {
        transform.position = _player.transform.position;
    }

    protected override IEnumerator DeActivateSkill()
    {
        _isCoroutineRunning = true;
        yield return _skillDurationTime;
        // 적용됐던 버프 효과 되돌리기
        StatusChange(false);
        gameObject.SetActive(false);
        _isCoroutineRunning = false;
    }

    private void OnDestroy()
    {
        // 버프 진행 중에 겜 종료 / 스킬 제거 등 발생 시
        if(_isCoroutineRunning)
        {
            StatusChange(false);
            StopAllCoroutines();
        }
    }

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
