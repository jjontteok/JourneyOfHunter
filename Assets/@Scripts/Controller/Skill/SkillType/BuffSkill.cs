using System.Collections;
using UnityEngine;

public class BuffSkill : ActiveSkill
{
    bool _isCoroutineRunning = false;
    ParticleSystem _particle;

    public override bool ActivateSkill(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        ProcessBuff(true);

        StartCoroutine(DeActivateSkill());
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _particle = GetComponentInChildren<ParticleSystem>();
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
        ProcessBuff(false);
        gameObject.SetActive(false);
        _isCoroutineRunning = false;
    }

    void ProcessBuff(bool flag)
    {
        float addition = SkillData.buffAmount;
        if (!flag)
        {
            addition *= -1;
        }
        _player.OnOffStatusUpgrade(SkillData.buffStatus, addition);
    }

    private void OnDestroy()
    {
        // 버프 진행 중에 겜 종료 / 스킬 제거 등 발생 시
        if(_isCoroutineRunning)
        {
            ProcessBuff(false);
        }
    }
}
