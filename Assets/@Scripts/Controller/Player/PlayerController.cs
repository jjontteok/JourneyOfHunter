using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    Rigidbody _rigidbody;
    SkillSystem _skillSystem;

    [SerializeField] PlayerData _playerData;
    [SerializeField] float _speed = 10f;

    // 데이터는 getter만 되도록?
    public PlayerData PlayerData {  get { return _playerData; } }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        Move();
        Attack();
    }

    void Initialize()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.PlayerSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterTag), LayerMask.NameToLayer(Define.MonsterSkillLayer));
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _skillSystem = GetComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();

        foreach (var slot in _skillSystem._slotList)
        {
            if (slot._skill.SkillData.skillType == Define.SkillType.RigidbodyTarget || slot._skill.SkillData.skillType == Define.SkillType.TransformTarget)
            {
                slot._skill.GetComponent<TargetSkill>().OnSkillSet += Rotate;
            }
        }
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
            _rigidbody.MovePosition(_rigidbody.position + movement.normalized * _speed * Time.deltaTime);

            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -23, 23);
            newPos.z = Mathf.Clamp(transform.position.z, 3, 115);
            transform.position = newPos;
            _animator.SetFloat(Define.Speed, movement.magnitude);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), _speed * Time.deltaTime);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat(Define.Speed, 0);
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
        float finalDamage = damage / _playerData.Def;
        _playerData.HP -= finalDamage;
        Debug.Log($"{name} Damaged: {finalDamage}");
        //if (_runtimeData.HP <= 0)
        //    Die();
    }
}