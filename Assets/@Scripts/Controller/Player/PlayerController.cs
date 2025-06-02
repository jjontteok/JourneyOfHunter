using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    Rigidbody _rigidbody;
    SkillSystem _skillSystem;

    [SerializeField] PlayerData _playerData;
    [SerializeField] float _speed;

    [SerializeField] Image _playerMpBar;

    // 데이터는 getter만 되도록?
    public PlayerData PlayerData { get { return _playerData; } }

    float _mp;
    float _hp;

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
        //Move();
        //Attack();
        Recover();
    }

    private void FixedUpdate()
    {
        Move();
        ClampPosition();
    }

    void Initialize()
    {
        _hp = _playerData.HP;
        _mp = _playerData.MP;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        // 기본 공격을 TransformTargetSkill로 쓸 경우 test
        _skillSystem = GetComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();
        if(_skillSystem.BasicSkillSlot.Skill.SkillData.skillType==Define.SkillType.TransformTarget)
        {
            _skillSystem.BasicSkillSlot.Skill.GetComponent<TargetSkill>().OnSkillSet += Rotate;
        }

        //_skillSystem.InitializeSkillSystem();

        //foreach (var slot in _skillSystem._activeSkillSlotList)
        //{
        //    if (slot.SkillData.skillType == Define.SkillType.RigidbodyTarget || slot.SkillData.skillType == Define.SkillType.TransformTarget)
        //    {
        //        slot.GetComponent<TargetSkill>().OnSkillSet += Rotate;
        //    }
        //}
    }

    void Move()
    {
        if (transform.position.z >= 113.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;
        }
        if (!_animator.GetBool(Define.IsAttacking) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(h, 0, v);
            _rigidbody.MovePosition(_rigidbody.position + movement.normalized * _speed * Time.fixedDeltaTime);
            
            _animator.SetFloat(Define.Speed, movement.magnitude);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), _speed * Time.deltaTime);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat(Define.Speed, 0);
        }
    }

    void ClampPosition()
    {
        Vector3 newPos = _rigidbody.position;
        newPos.x = Mathf.Clamp(newPos.x, -23, 23);
        newPos.z = Mathf.Clamp(newPos.z, 3, 115);
        _rigidbody.position = newPos;
    }

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

    // * 방어력 적용 데미지 계산 메서드
    public void GetDamaged(float damage)
    {
        float finalDamage = CalculateFinalDamage(damage, _playerData.Def);
        _hp -= finalDamage;
        Debug.Log($"Damaged: {finalDamage}, Current Player HP: {_hp}");
        //if (_runtimeData.HP <= 0)
        //    Die();
    }

    float CalculateFinalDamage(float damage, float def)
    {
        return damage * (1 - def / Define.MaxDef);
    }
}