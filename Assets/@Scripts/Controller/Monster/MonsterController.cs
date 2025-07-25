using UnityEngine;
using extension;
using System;

// * Monster Status 구조체
//- Scriptable Object의 런타임 복사용으로 활용
public struct MonsterStatus
{
    public string Name;                 // 이름
    public string Description;          // 설명
    public float Atk;                   // 공격력
    public float Def;                   // 방어력
    public float MaxHP;                 // 최대 체력
    public float CurrentHP;             // 현재 체력
    public float Speed;                 // 이동 속도
    public float AttackSpeed;           // 공격 속도
    public float MoveRange;             // 이동 범위
    public float AttackRange;           // 공격 범위

    public MonsterStatus(MonsterData monsterData)
    {
        Name = monsterData.Name;
        Description = monsterData.Description;
        Atk = monsterData.Atk;
        Def = monsterData.Def;
        MaxHP = monsterData.HP;
        CurrentHP = monsterData.HP;
        Speed = monsterData.Speed;
        AttackSpeed = monsterData.AttackSpeed;
        MoveRange = monsterData.MoveRange;
        AttackRange = monsterData.AttackRange;
    }
}

// * MonsterController 스크립트
//- 이동, 공격, 충돌, 사망
public abstract class MonsterController : MonoBehaviour, IDamageable
{
    [SerializeField] protected MonsterData _monsterData;
    [SerializeField] protected GameObject _target;

    public static event Action OnMonsterDead;

    [SerializeField] protected MonsterStatus _runtimeData;
    protected Animator _animator;
    protected AttackRangeController _attackRangeController;
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        MoveToTarget(_target.transform.position);
    }

    protected virtual void OnDisable()
    {
        _runtimeData = new MonsterStatus(_monsterData);
        _runtimeData.CurrentHP = _runtimeData.MaxHP;
    }

    // * 초기화 메서드
    //- 컴포넌트 연결 및 공격 범위 오브젝트 생성
    //- 공격 이벤트에 공격 메서드 구독
    public virtual void Initialize()
    {
        _runtimeData = new MonsterStatus(_monsterData);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _target = PlayerManager.Instance.Player.gameObject;

        GameObject attackRange = new GameObject("AttackRange");
        attackRange.layer = LayerMask.NameToLayer(Define.MonsterAttackRangeLayer);
        attackRange.transform.parent = this.gameObject.transform;
        _attackRangeController = attackRange.GetOrAddComponent<AttackRangeController>();
        _attackRangeController.Initialize(_runtimeData.AttackRange);
        _attackRangeController.OnAttack += Attack;
        _attackRangeController.OffAttack += EndAttack;

        FieldManager.Instance.OnUpgradeMonsterStatus += UpgradeStatus;
    }

    // 타겟 이동 메서드
    public virtual void MoveToTarget(Vector3 targetPos)
    {
        if (!_animator.GetBool(Define.IsAttacking))
        {
            Vector3 targetDir = (targetPos - transform.position).normalized;
            targetDir.y = 0;
            transform.position += targetDir * _runtimeData.Speed * Time.deltaTime;
            Quaternion toRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10 * Time.deltaTime);

            _animator.SetFloat(Define.WalkSpeed, _runtimeData.Speed / 2);
        }
        else
        {
            _animator.SetFloat(Define.WalkSpeed, 0);
        }
    }

    // * 공격 메서드
    //- 추후 스킬 사용 몬스터 대비 가상함수로 구현
    public virtual void Attack()
    {
        if (!_animator.GetBool(Define.IsAttacking))
        {
            _animator.SetBool(Define.IsAttacking, true);
            _animator.SetTrigger(Define.Attack);
        }
    }

    // * 공격 종료 메서드
    //- 공격 플래그 false 및 종료 트리거 발생
    public virtual void EndAttack()
    {
        _animator.SetBool(Define.IsAttacking, false);
    }

    // * 방어력 적용 데미지 계산 메서드
    public virtual void GetDamage(float damage)
    {
        float finalDamage = CalculateFinalDamage(damage, _runtimeData.Def);
        _runtimeData.CurrentHP -= finalDamage;
        DamageTextEvent.Invoke(Util.GetDamageTextPosition(gameObject.GetComponent<Collider>()), finalDamage, false);
        if (_runtimeData.CurrentHP <= 0)
            Die();
    }

    public float CalculateFinalDamage(float damage, float def)
    {
        float calc = damage - def;
        return calc > 0 ? calc : 0;
    }

    // * 사망 메서드
    //- 오브젝트 풀링 대비 비활성화 처리
    public virtual void Die()
    {
        OnMonsterDead?.Invoke();
        gameObject.SetActive(false);
        PlayerManager.Instance.Player.ReleaseTarget();
    }

    protected abstract void UpgradeStatus(float amount);
}
