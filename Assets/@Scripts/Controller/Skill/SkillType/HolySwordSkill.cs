using System;
using System.Collections;
using UnityEngine;

public class HolySwordSkill : AreaTargetSkill, IRotationSkill, IDirectionSkill, IUltimateSkill
{
    [SerializeField] GameObject _lastExplosion;
    ParticleSystem _particle;
    Animator _animator;
    int _animationHash;
    protected Vector3 _direction;

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
            StartCoroutine(CoActivateSkillwithMotion(pos));

            return true;
        }
        return false;
    }

    public IEnumerator CoActivateSkillwithMotion(Vector3 pos)
    {
        // 해당 궁극기의 모션 실행해주고
        _animator.SetTrigger(_animationHash);
        SetDirection();
        OnActivateSkill?.Invoke(_direction);
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

    public void SetDirection()
    {
        // 플레이어 스킬이 아니면(==몬스터 스킬이면) 타겟을 향해 설정
        if (!SkillData.IsPlayerSkill)
        {
            Vector3 dir = _target.position - transform.position;
            dir.y = 0;
            _direction = dir.normalized;
        }
        else
        {
            //타겟 방향으로 스킬 방향 설정
            //스킬이 땅으로 박히지 않도록 높이 맞춰주기
            Vector3 dir = _target.position - transform.position;
            dir.y = 0;
            _direction = dir.normalized;
            //// 자동 모드면 가까운 적을 향해 방향 설정
            //if (PlayerManager.Instance.IsAuto)
            //{
            //    //타겟 방향으로 스킬 방향 설정
            //    //스킬이 땅으로 박히지 않도록 높이 맞춰주기
            //    Vector3 dir = _target.position - transform.position;
            //    dir.y = 0;
            //    _direction = dir.normalized;
            //}
            //// 수동 모드면 현재 플레이어가 바라보는 방향으로 설정
            //else
            //{
            //    _direction = PlayerManager.Instance.Player.transform.TransformDirection(Vector3.forward);
            //}
        }
        //transform.rotation = Quaternion.LookRotation(_direction);
    }
}
