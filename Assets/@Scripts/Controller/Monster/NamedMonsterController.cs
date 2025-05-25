using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NamedMonsterController : MonsterController
{
    [SerializeField] private float _closeAttackLimit = 5f;
    [SerializeField] private bool _isMoveToOrigin;
    [SerializeField] private Vector3 _originPos;
    [SerializeField] private SkillData _explosionSkillData;
    [SerializeField] private SkillData _bulletSkillData;
    [SerializeField] private GameObject _bulletSkillPrefab;
    float _coolTime = 0.0f;

    private MoveRangeController _moveRangeController;

    private RigidbodyTargetSkill _skill;


    private void Awake()
    {
        //_playableDirector = GetComponent<PlayableDirector>();
        //_playableDirector.Play();
        _originPos = transform.position;
        base.Initialize();
        GameObject moveRange = new GameObject("MoveRange");
        moveRange.transform.parent = this.gameObject.transform;
        _moveRangeController = moveRange.GetOrAddComponent<MoveRangeController>();
        _moveRangeController.Intiailize(_runtimeData.MoveRange);

        _skill = Instantiate(_bulletSkillPrefab).GetComponent<RigidbodyTargetSkill>();
        _skill.Initialize();
        _skill.gameObject.SetActive(false);

        //������Ʈ �Ŵ������� ������ ���� -> ���� �ʿ�
    }

    private void OnEnable()
    {
        _isMoveToOrigin = false;
        _moveRangeController.OnMoveToTarget += MoveToTarget;
        _moveRangeController.OnMoveToOrigin += MoveToOrigin;
        _attackRangeController.OffAttack += EndAttack;
    }

    private void Update()
    {
        RotateNamedMonster();
        CheckMoveToOrigin();
       // Debug.Log($"���ӵ� ���Ϳ� �÷��̾���� �Ÿ� : {Vector3.Distance(transform.position, _target.transform.position)}");
    }

    void RotateNamedMonster()
    {
        if (!_isMoveToOrigin)
        {
            Vector3 dir = _target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    //���� �ڸ��� ���� ���ϴ� �Լ�
    void CheckMoveToOrigin()
    {
        if (_isMoveToOrigin)
        {
            MoveToTarget(_originPos);
            if ((transform.position - _originPos).sqrMagnitude < 0.1)
            {
                _isMoveToOrigin = false;
                _animator.SetFloat(Define.WalkSpeed, 0);
                transform.rotation = Quaternion.LookRotation(-Vector3.forward);
            }
            else if (Vector3.Distance(transform.position, _target.transform.position) < _runtimeData.MoveRange)
            {
                _isMoveToOrigin = false;
            }
        }

    }

    //moveRange ���� �ȿ� ���� �� ����Ǵ� �Լ�
    //5~8���� �������� �̵�
    void MoveToTarget()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        if(distance > _closeAttackLimit && distance <= _runtimeData.MoveRange)
        {
            MoveToTarget(_target.transform.position);
        }
        else
        {
            _animator.SetFloat(Define.WalkSpeed, 0);
        }
    }

    //moveRange ���� �ۿ� ���� �� ����Ǵ� �Լ�
    void MoveToOrigin()
    {
        if(_bulletSkillPrefab.activeSelf)
            DeactiveLongAttack();
        _isMoveToOrigin = true;
    }

    //���� ���� ���� ������ ��� ȣ��� �Լ�
    public override void Attack()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);

        if (!_isMoveToOrigin)
        {

            //0~3���� �������� �̵����� �ʰ� ����
            if (distance < _closeAttackLimit)
            {
                _animator.SetTrigger(Define.CloseAttack);
                DeactiveLongAttack();
            }
            
            else if (distance > _runtimeData.MoveRange && distance < _runtimeData.AttackRange)
            {
               _coolTime += Time.deltaTime;
               if (_coolTime > 3f)
               {
                    _coolTime = 0;
                    ActiveLongAttack(distance);
               }
            }
        }
    }

    //���Ÿ� ���� Ȱ��ȭ
    void ActiveLongAttack(float distance)
    {
        _bulletSkillData.force = distance + 10;
        _animator.SetTrigger(Define.LongAttack);
        _skill.ActivateSkill(_target.transform, transform.position + Vector3.up * 2);
    }

    //���Ÿ� ���� ��Ȱ��ȭ
    void DeactiveLongAttack()
    {

    }

    //���Ÿ� ���� ������ ����� ���� ����
    public override void EndAttack()
    {
        _animator.SetTrigger(Define.Idle);
    }
}
