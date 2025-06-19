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
        _playerRigidbody = _playerController.GetComponent<Rigidbody>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {

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
            OnSkillActivated?.Invoke(SkillData.durationTime);
            StartCoroutine(CoAfterEffect());
            return true;
        }

        return false;
    }

    public override void MoveSkillCollider()
    {
        if (Vector3.Distance(_coll.transform.position, _originPos) < _skillData.targetDistance)
        {
            //_playerController.transform.Translate(_fixedDirection * _skillData.speed * Time.deltaTime, Space.World);
            _playerRigidbody.MovePosition(_playerRigidbody.position + _fixedDirection * _skillData.speed * Time.fixedDeltaTime);
            //_coll.transform.position = _playerController.transform.position;
            _rigidbody.MovePosition(_rigidbody.position + _fixedDirection * _skillData.speed * 1.3f * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + _direction * _skillData.targetDistance + Vector3.up * 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _fixedDirection * _skillData.targetDistance);
    }

    IEnumerator CoAfterEffect()
    {
        //yield return new WaitForSeconds(SkillData.durationTime - 0.1f);
        yield return null;
        GameObject particle = Instantiate(_afterEffect, _coll.transform.position, Quaternion.Euler(-90, 0, 0),_coll.transform);
        PlayerManager.Instance.Player.GetComponent<Animator>().SetTrigger("SkillAttack");
        Destroy(particle, 0.5f);
    }
}
