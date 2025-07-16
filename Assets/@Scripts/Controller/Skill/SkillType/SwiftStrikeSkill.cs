using System;
using System.Collections;
using UnityEngine;

public class SwiftStrikeSkill : RotationTargetSkill, ICharacterMovingSkill
{
    Vector3 _originPos;
    Vector3 _fixedDirection;
    Rigidbody _playerRigidbody;
    Rigidbody _rigidbody;
    [SerializeField] GameObject _afterEffect;

    public event Action<float> OnSkillActivated;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _playerRigidbody = _player.GetComponent<Rigidbody>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        // 부모 객체에서의 Update 효과 무시하기 위함
    }

    private void FixedUpdate()
    {
        MoveSkillCollider();
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        if (base.ActivateSkill(pos))
        {
            _originPos = transform.position;
            Vector3 tmp = _direction;
            tmp.y = 0f;
            _fixedDirection = tmp;
            OnSkillActivated?.Invoke(SkillData.DurationTime);
            StartCoroutine(CoAfterEffect());
            return true;
        }

        return false;
    }

    public override void MoveSkillCollider()
    {
        if (Vector3.Distance(_coll.transform.position, _originPos) < _skillData.TargetDistance)
        {
            //_playerController.transform.Translate(_fixedDirection * _skillData.Speed * Time.deltaTime, Space.World);
            _playerRigidbody.MovePosition(_playerRigidbody.position + _fixedDirection * _skillData.Speed * Time.fixedDeltaTime);
            //_coll.transform.position = _playerController.transform.position;
            _rigidbody.MovePosition(_rigidbody.position + _fixedDirection * _skillData.Speed * 1.3f * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 linearVelo = _playerRigidbody.linearVelocity;
            linearVelo.y = 0;
            _playerRigidbody.linearVelocity = linearVelo;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + _direction * _skillData.TargetDistance + Vector3.up * 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _fixedDirection * _skillData.TargetDistance);
    }

    IEnumerator CoAfterEffect()
    {
        yield return null;
        GameObject particle = Instantiate(_afterEffect, _coll.transform);
        particle.transform.localPosition = Vector3.up;
        particle.transform.localEulerAngles = new Vector3(-90, 0, 0);
        //PlayerManager.Instance.Player.GetComponent<Animator>().SetTrigger("SkillAttack");
        PlayerManager.Instance.Player.GetComponent<Animator>().SetTrigger(Define.Attack);
        Destroy(particle, 0.5f);
    }
}
