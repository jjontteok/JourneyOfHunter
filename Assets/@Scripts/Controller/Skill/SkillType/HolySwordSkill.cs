using System;
using System.Collections;
using UnityEngine;

public class HolySwordSkill : AreaTargetSkill, IRotationSkill, IUltimateSkill
{
    [SerializeField] GameObject _lastExplosion;
    ParticleSystem _particle;
    Animator _animator;
    int _animationHash;

    public event Action<Vector3> OnActivateSkill;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        InitializeAnimationSetting();
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        if(base.ActivateSkill(pos))
        {
            // 눈에서 안 보이게 꼼수
            _particle.Stop();
            _lastExplosion.SetActive(false);
            _coll.gameObject.SetActive(false);
            //transform.position = new Vector3(0, 500f, 0);
            StartCoroutine(CoActivateSkillwithMotion(pos));

            return true;
        }
        return false;
    }

    public IEnumerator CoActivateSkillwithMotion(Vector3 pos)
    {
        // 해당 궁극기의 모션 실행해주고
        _animator.SetTrigger(_animationHash);
        OnActivateSkill?.Invoke(_target.position);
        // 특정 타이밍에 스킬 실행
        yield return new WaitUntil(() => _animator.GetBool(Define.UltimateReady));
        _particle.Play();
        _coll.gameObject.SetActive(true);
        Invoke("ActivateLastExplosion", 0.5f);
    }

    public void InitializeAnimationSetting()
    {
        _animator = _player.GetComponent<Animator>();
        _animationHash = Animator.StringToHash(_skillData.SkillAnimationName);
        _particle = GetComponent<ParticleSystem>();
    }

    void ActivateLastExplosion()
    {
        _lastExplosion.SetActive(true);
    }
}
