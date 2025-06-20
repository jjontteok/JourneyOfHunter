using System.Collections;
using UnityEngine;

public class BuffSkill : ActiveSkill, IStatusChangeSkill
{
    bool _isCoroutineRunning = false;
    bool _isMultiply;
    ParticleSystem _particle;

    public override bool ActivateSkill(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        StatusChange(true, _isMultiply);

        StartCoroutine(DeActivateSkill());
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _particle = GetComponentInChildren<ParticleSystem>();
        switch (_skillData.buffStatus)
        {
            case Define.StatusType.Atk:
            case Define.StatusType.Def:
            case Define.StatusType.Damage:
            case Define.StatusType.HP:
                _isMultiply = false;
                break;

            case Define.StatusType.HPRecoveryPerSec:
            case Define.StatusType.CoolTimeDecrease:
                _isMultiply = true;
                break;

            default:
                break;
        }
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
        StatusChange(false,false);
        gameObject.SetActive(false);
        _isCoroutineRunning = false;
    }

    private void OnDestroy()
    {
        // 버프 진행 중에 겜 종료 / 스킬 제거 등 발생 시
        if(_isCoroutineRunning)
        {
            StatusChange(false, _isMultiply);
        }
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
