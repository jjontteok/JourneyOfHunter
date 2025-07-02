using System.Collections;
using UnityEngine;

public class IceAgeSkill : AoENonTargetSkill, IUltimateSkill
{
    Animator _animator;
    int _animationHash;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        InitializeAnimationSetting();
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        gameObject.SetActive(true);
        // 눈에서 안 보이게 꼼수
        transform.position = new Vector3(0, 500f, 0);
        StartCoroutine(CoActivateSkillwithMotion(pos));

        return true;
    }

    public IEnumerator CoActivateSkillwithMotion(Vector3 pos)
    {
        // 해당 궁극기의 모션 실행해주고
        _animator.SetTrigger(_animationHash);
        // 특정 타이밍에 스킬 실행
        yield return new WaitUntil(() => _animator.GetBool(Define.UltimateReady));
        base.ActivateSkill(pos);
    }

    public void InitializeAnimationSetting()
    {
        _animator = _player.GetComponent<Animator>();
        _animationHash = Animator.StringToHash(_skillData.SkillAnimationName);
    }
}
