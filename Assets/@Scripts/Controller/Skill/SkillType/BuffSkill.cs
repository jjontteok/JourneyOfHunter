using System;
using System.Collections;
using UnityEngine;

public class BuffSkill : ActiveSkill, IStatusChangeSkill, ICheckActivation, IStatusEffectSkill
{
    bool _isCoroutineRunning = false;
    bool _isOn = false;

    public event Action<Sprite, bool> OnStatusEffect;

    public override bool ActivateSkill(Vector3 pos)
    {
        if (IsActivatePossible(pos))
        {
            gameObject.SetActive(true);
            transform.position = pos;
            StatusChange(true);
            _isOn = true;
            OnStatusEffect?.Invoke(_skillData.IconImage, true);

            StartCoroutine(DeActivateSkill());
            return true;
        }
        return false;
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
        _isOn = false;
        OnStatusEffect?.Invoke(_skillData.IconImage, false);
        gameObject.SetActive(false);
        _isCoroutineRunning = false;
    }

    private void OnDestroy()
    {
        // 버프 진행 중에 겜 종료 / 스킬 제거 등 발생 시
        if (_isCoroutineRunning)
        {
            StatusChange(false);
            _isOn = false;
            OnStatusEffect?.Invoke(_skillData.IconImage, false);
            StopAllCoroutines();
        }
    }

    public void StatusChange(bool flag)
    {
        float coeff = SkillData.BuffAmount;

        if (!flag)
        {
            coeff *= -1;
        }

        _player.OnOffStatusUpgrade(SkillData.BuffStatus, coeff);
    }

    public bool IsActivatePossible(Vector3 pos)
    {
        if (_isOn)
            return false;
        // 수동 모드일 땐 타겟 유무 상관없이 그냥 발사
        if (SkillData.IsPlayerSkill && !PlayerManager.Instance.IsAuto)
            return true;
        return FieldManager.Instance.CurrentEventType == Define.JourneyType.Dungeon && !_isOn;
    }
}
