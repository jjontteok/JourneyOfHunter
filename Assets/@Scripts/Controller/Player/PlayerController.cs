using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    Animator _animator;
    Rigidbody _rigidbody;
    SkillSystem _skillSystem;
    [SerializeField] Transform _target;

    Vector3 _direction;
    float _mp;
    float _hp;
    float _shortestSkillDistance;   //자동일 때, 이동 멈추는 범위
    bool _isAuto;                   //자동 여부
    bool _isAutoMoving;             //자동일 때, 타겟 없을 시 다음 스테이지 이동 여부

    public bool IsAuto
    {
        get { return _isAuto; }
        set
        {
            _isAuto = value;
            _skillSystem.IsAuto = value;
        }
    }

    public Transform Target { get { return _target; } }

    [SerializeField] PlayerData _playerData;
    [SerializeField] float _speed;

    [SerializeField] Image _playerMpBar;

    // 데이터는 getter만 되도록?
    public PlayerData PlayerData { get { return _playerData; } }


    public float MP
    {
        get { return _mp; }
        set
        {
            _mp = value;
            _playerMpBar.fillAmount = _mp / _playerData.MP;
        }
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        Recover();
        SkillInventoryOnOff();
    }

    private void FixedUpdate()
    {
        //GetMovementByKeyBoard();
        Move();
    }

    void Initialize()
    {
        _hp = _playerData.HP;
        _mp = _playerData.MP;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _skillSystem = GetComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();
        _skillSystem.BasicSkillSlot.Skill.GetComponent<TransformTargetSkill>().OnSkillSet += Rotate;

        SkillManager.Instance.LockIconSlots(_playerData.UnlockedSkillSlotCount);
    }

    #region Player Moving

    public void Move()
    {
        if (transform.position.z >= 113.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;
        }

        // 자동 모드일 때
        if (_isAuto)
        {
            // 타겟 없으면
            if (_target == null || !_target.gameObject.activeSelf)
            {
                SetTarget();
                // 찾았는데도 없으면 다음 스테이지 자동 이동?
                if (_target == null)
                {
                    return;
                }
                Debug.Log($"Current Target: {_target.name}");
            }
            //타겟과 거리가 가까워지면 정지
            if (Vector3.Distance(transform.position, _target.position) <= _shortestSkillDistance)
            {
                _direction = Vector3.zero;
                _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);

                _animator.SetFloat(Define.Speed, 0);
            }
            else
            {
                _direction = _target.position - transform.position;
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _speed * Time.fixedDeltaTime);


                _animator.SetFloat(Define.Speed, _direction.magnitude);
                //타겟 바라보게 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _speed * Time.deltaTime);
            }
        }
        else
        {
            if (!_animator.GetBool(Define.IsAttacking) && _direction != Vector3.zero)
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _speed * Time.fixedDeltaTime);

                _animator.SetFloat(Define.Speed, _direction.magnitude);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _speed * Time.deltaTime);
            }
            else
            {
                _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
                _animator.SetFloat(Define.Speed, 0);
            }
        }
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 newPos = _rigidbody.position;
        newPos.x = Mathf.Clamp(newPos.x, -23, 23);
        newPos.z = Mathf.Clamp(newPos.z, 3, 115);
        _rigidbody.position = newPos;
    }

    public void Attack()
    {
        if (!_animator.GetBool(Define.IsAttacking) && Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger(Define.Attack);
        }
    }

    public void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void SetMoveDirectionByKeyBoard()
    {
        Vector3 movement;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            movement = new Vector3(h, 0, v);
        }
        else
        {
            movement = Vector3.zero;
        }
        _direction = movement;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _direction = dir;
    }
    #endregion

    #region Player Utility
    void Recover()
    {
        _hp += _playerData.HPRecoveryPerSec * Time.deltaTime;
        if (_hp > _playerData.HP)
        {
            _hp = _playerData.HP;
        }
        MP += _playerData.MPRecoveryPerSec * Time.deltaTime;
        if (MP > _playerData.MP)
        {
            MP = _playerData.MP;
        }
    }

    void SetTarget()
    {
        _target = Util.GetNearestTarget(transform.position, _shortestSkillDistance)?.transform;
        if (_target == null || !_target.gameObject.activeSelf)
        {
            //쵸비상 몬스터 풀 어케 가져옴
            _target = Util.GetNearestTarget(transform.position, 100f).transform;
            if (_target == null)
            {
                Debug.Log("No target on field!!!");
                return;
            }
        }
    }

    void SkillInventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PopupUIManager.Instance.ActivateSkillInventoryPanel();
        }
    }

    public void SetAuto(bool flag)
    {
        IsAuto = flag;
        _direction = Vector3.zero;
    }

    public void SetShortestSkillDistance(float distance)
    {
        _shortestSkillDistance = distance;
    }
    #endregion

    #region IDamageable Methods
    // * 방어력 적용 데미지 계산 메서드
    public void GetDamaged(float damage)
    {
        float finalDamage = CalculateFinalDamage(damage, _playerData.Def);
        _hp -= finalDamage;
        Debug.Log($"Damaged: {finalDamage}, Current Player HP: {_hp}");
        //if (_runtimeData.HP <= 0)
        //    Die();
    }

    public float CalculateFinalDamage(float damage, float def)
    {
        return damage * (1 - def / Define.MaxDef);
    }
    #endregion
}