using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct PlayerStatus
{
    public float Atk;
    public float Def;
    public float HP;
    public float HPRecoveryPerSec;
    public float CoolTimeDecrease;
    public float JourneyExp;
    public float Speed;

    public PlayerStatus(PlayerData playerData)
    {
        Atk = playerData.Atk;
        Def = playerData.Def;
        HP = playerData.HP;
        HPRecoveryPerSec = playerData.HPRecoveryPerSec;
        CoolTimeDecrease = playerData.CoolTimeDecrease;
        JourneyExp = playerData.JourneyExp;
        Speed = playerData.Speed;
    }

    public float GetCoolTimeDecrease()
    {
        return CoolTimeDecrease;
    }
}

public class PlayerController : MonoBehaviour, IDamageable
{
    Animator _animator;
    Rigidbody _rigidbody;
    AudioSource _footstepSound;
    [SerializeField] Transform _target;
    float _targetDistance;

    public Action<float, float> OnHPValueChanged;
    public Action<float, float> OnMPValueChanged;
    public Action<float> OnJourneyExpChanged;
    public Action OnPlayerCrossed;
    public Action OnPlayerDead;
    public Action OnAutoMerchantAppear;

    [SerializeField] PlayerStatus _runtimeData;
    Vector3 _direction;
    float _hp;

    float _shortestSkillDistance;       //자동일 때, 이동 멈추는 범위

    float _atkBuff = 0;                 //던전 클리어 실패 시 주어질 버프
    float _hpBuff = 0;

    bool _isSwifting;                   //질풍참 사용 여부
    bool _isKeyBoard;
    bool _isJoyStick;

    public event Action OnPageUp;
    public event Action OnPageDown;

    public event Action OnAutoOff;
    public Action OnAutoDungeonChallenge;

    [SerializeField] PlayerData _playerData;
    [SerializeField] PlayerInventoryData _playerInventoryData;
    private Inventory _inventory;

    #region Properties

    public PlayerInventoryData PlayerInventoryData { get { return _playerInventoryData; } }

    public bool IsKeyBoard
    {
        get { return _isKeyBoard; }
        set
        {
            // 자동 이동 중인데 키보드 입력할 경우 자동 이동 종료
            if (PlayerManager.Instance.IsAuto && value)
            {
                PlayerManager.Instance.IsAuto = false;
                PlayerManager.Instance.IsAutoMoving = false;
                OnAutoOff?.Invoke();
            }
            _isKeyBoard = value;
        }
    }
    public bool IsJoyStick
    {
        get { return _isJoyStick; }
        set
        {
            // 자동 이동 중인데 조이스틱 입력할 경우 자동 이동 종료
            if (PlayerManager.Instance.IsAuto && value)
            {
                PlayerManager.Instance.IsAuto = false;
                PlayerManager.Instance.IsAutoMoving = false;
                OnAutoOff?.Invoke();
            }
            _isJoyStick = value;
        }
    }
    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public AudioSource AudioSource
    {
        get { return _footstepSound; }
    }

    // 데이터는 getter만 되도록?
    public PlayerData PlayerData { get { return _playerData; } }
    public Inventory Inventory { get { return _inventory; } }
    public PlayerStatus PlayerStatus { get { return _runtimeData; } }

    public float HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            OnHPValueChanged?.Invoke(_hp, _playerData.HP);
        }
    }

    public float JourneyExp
    {
        get { return _playerData.JourneyExp; }
        set
        {
            int changeExpCount = 0;
            _playerData.JourneyExp = value;

            //해당 레벨의 맥스 값보다 현재 메달 값이 높으면 
            while (_playerData.JourneyRankData.MaxJourneyExp <= _playerData.JourneyExp)
            {
                changeExpCount++;
                _playerData.JourneyRankData =
                    ObjectManager.Instance.JourneyRankResourceList[(_playerData.JourneyRankData.Index + 1).ToString()];
            }
        }
    }
    #endregion

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        Recover();
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            _playerData.UnlockedSkillSlotCount = Mathf.Max(_playerData.UnlockedSkillSlotCount - 1, Define.MinUnlockedSkillSlotCount);
            OnPageDown?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            _playerData.UnlockedSkillSlotCount = Mathf.Min(_playerData.UnlockedSkillSlotCount + 1, Define.MaxUnlockedSkillSlotCount);
            OnPageUp?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        SetMoveDirectionByKeyBoard();
        Move();
    }

    void Initialize()
    {
        _hp = _playerData.HP;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _footstepSound = GetComponent<AudioSource>();
        _runtimeData = new PlayerStatus(_playerData);
        _playerData.JourneyRankData =
            ObjectManager.Instance.JourneyRankResourceList[_playerData.JourneyRankData.Index.ToString()];
        _inventory = new Inventory(_playerInventoryData);
    }

    #region Player Moving

    public void GainJourneyExp(int amount = default)
    {
        float journeyExp = 5; //기본 증가량

        //구역을 지날 때에는 해당 메달의 랭크대로 여정의 증표 증가
        if (amount == default)
            journeyExp *= _playerData.JourneyRankData.Index;
        //그냥 일반 획득이라면 스테이지대로 여정의 증표 증가
        else
            journeyExp = amount;

        JourneyExp += journeyExp;
        OnJourneyExpChanged?.Invoke(journeyExp);
        Debug.Log("여정의 증표 획득 " + journeyExp + "현재 여정의 증표 "+JourneyExp);
    }

    public void Move()
    {
        if (!PlayerManager.Instance.IsGameStart) 
            return;

        // 죽은 상태이거나 공격 모션 중일 땐 움직이지 않도록
        if (_animator.GetInteger(Define.DieType) > 0 || _animator.GetBool(Define.IsAttacking))
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat(Define.Speed, 0);
            //_footstepSound.Stop();
            return;
        }
        //던전에 들어가지 않았을 때 플레이어의 위치를 이동시킴
        if (!FieldManager.Instance.DungeonController.IsDungeonExist && transform.position.z >= 113.2)
        {
            Vector3 pos = transform.position;
            // z = 5 로 이동
            pos.z -= Define.TeleportDistance;
            transform.position = pos;

            //구역 통과 시 여정 경험치? 증가
            GainJourneyExp();

            OnPlayerCrossed?.Invoke();
        }

        // 자동 모드일 때
        if (!_isSwifting && PlayerManager.Instance.IsAuto && !_animator.GetBool(Define.IsAttacking))
        {
            if (PlayerManager.Instance.IsAutoMoving)
            {
                MoveAlongRoad();
            }
            else
            {
                // IsAutoMoving == false && IsClear == true ==> 정지해 있기
                if (FieldManager.Instance.IsClear)
                {
                    StandStill();
                }
                // IsAutoMoving == false && IsClear == false ==> 타겟을 향해 움직이기
                else
                {
                    if (_target == null)
                    {
                        SetTarget();
                    }
                    MoveToTarget();
                }
            }

            // 타겟 생기면 IsAutoMoving=false
            //if (PlayerManager.Instance.IsAutoMoving)
            //{
            //    // 필드 클리어한 상태면 길따라
            //    if(FieldManager.Instance.IsClear)
            //    {
            //        MoveAlongRoad();
            //    }
            //    else
            //    {
            //        SetTarget();
            //        if (_target == null)
            //        {
            //            MoveAlongRoad();
            //        }
            //    }
            //}
            //else
            //{
            //    if (_target == null)
            //    {
            //        SetTarget();
            //        if (_target == null)
            //        {
            //            MoveAlongRoad();
            //        }
            //    }
            //    else
            //    {
            //        if (_target == null || !_target.gameObject.activeSelf)
            //        {
            //            SetTarget();
            //        }
            //        if (((_target != null && !_target.CompareTag(Define.PortalTag)) && FieldManager.Instance.CurrentEventType == Define.JourneyType.Dungeon) || FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
            //        {
            //            MoveToTarget(_shortestSkillDistance);
            //        }
            //        else
            //        {
            //            MoveToTarget(0.5f);
            //        }
            //    }
            //}
        }
        // 수동 모드일 때
        else
        {
            if (!_animator.GetBool(Define.IsAttacking) && _direction != Vector3.zero)
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _runtimeData.Speed * 1 * Time.fixedDeltaTime);

                _animator.SetFloat(Define.Speed, _direction.magnitude);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _runtimeData.Speed * Time.deltaTime);
            }
            else
            {
                StandStill();
            }
        }
        ClampPosition();
    }

    // target과의 거리가 distance 이하가 될 때까지 움직임
    bool MoveToTarget()
    {
        if (_target == null)
        {
            // 타겟 없으면 일단 앞으로 전진
            MoveAlongRoad();
            return true;
        }

        //float distance;
        //// 몬스터 타겟이거나, 보물상자 타겟일 경우
        //if (_target.CompareTag(Define.MonsterTag) || FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
        //{
        //    distance=_shortestSkillDistance;
        //}
        //// 포탈, 오브젝트 등 접촉해야 하는 경우엔 거리를 짧게 설정
        //else
        //{
        //    distance=0.5f;
        //}
        //타겟과 거리가 distance 이하로 되면 정지
        Vector3 targetPos = _target.position;
        targetPos.y = 0;
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        float currentDistance = Vector3.Distance(targetPos, playerPos);
        if (currentDistance <= _targetDistance)
        {
            //_direction = Vector3.zero;
            //_rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);

            //_animator.SetFloat(Define.Speed, 0);
            StandStill();
            return false;
        }
        else
        {
            //if (PlayerManager.Instance.IsAutoMoving && Mathf.Abs(_rigidbody.position.x) > 0.1f)
            //{
            //    _direction = new Vector3(_target.position.x - transform.position.x, 0, 1);
            //}
            //else
            //{
            //    _direction = _target.position - transform.position;
            //    _direction.y = 0;
            //}
            _direction = _target.position - transform.position;
            _direction.y = 0;
            // 공격 모션 중이지 않을 때 이동
            if (!_animator.GetBool(Define.IsAttacking))
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _runtimeData.Speed * Time.fixedDeltaTime);

                _animator.SetFloat(Define.Speed, _direction.magnitude);
                //타겟 바라보게 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _runtimeData.Speed * Time.deltaTime);
            }

            return true;
        }
    }

    // 가운데 길 따라 이동
    void MoveAlongRoad()
    {
        if (Mathf.Abs(_rigidbody.position.x) > 0.1f)
        {
            _direction = new Vector3(-_rigidbody.position.x, 0, 1).normalized;
        }
        else
        {
            _direction = Vector3.forward;
        }
        _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _runtimeData.Speed * Time.fixedDeltaTime);
        _animator.SetFloat(Define.Speed, _direction.magnitude);
        //타겟 바라보게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _runtimeData.Speed * Time.deltaTime);
    }

    // 가만히 서있기
    void StandStill()
    {
        _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        _animator.SetFloat(Define.Speed, 0);
    }

    void ClampPosition()
    {
        Vector3 newPos = _rigidbody.position;
        newPos.x = Mathf.Clamp(newPos.x, -23, 23);
        newPos.z = Mathf.Clamp(newPos.z, -100, 300);
        _rigidbody.position = newPos;
    }

    public void Rotate(Vector3 direction)
    {
        direction.y = transform.forward.y;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void SetMoveDirectionByKeyBoard()
    {
        // 조이스틱 이동 중엔 키보드 이동 불가
        if (_isJoyStick)
            return;
        Vector3 movement;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            IsKeyBoard = true;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            movement = new Vector3(h, 0, v);
        }
        else
        {
            IsKeyBoard = false;
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
        HP += _runtimeData.HPRecoveryPerSec * Time.deltaTime;
        if (_hp > _runtimeData.HP)
        {
            _hp = _runtimeData.HP;
        }
    }

    void SetTarget()
    {
        // 던전인 경우, 몬스터 찾기
        if (FieldManager.Instance.CurrentEventType == Define.JourneyType.Dungeon)
        {
            // 우선 최단거리 기준으로 찾아보고
            _target = Util.GetNearestTarget(transform.position, _shortestSkillDistance)?.transform;
            // 없으면 
            if (_target == null || !_target.gameObject.activeSelf)
            {
                //쵸비상 몬스터 풀 어케 가져옴
                //stage info에서 현재 스테이지의 몬스터 정보를 받아와서 이름으로
                _target = Util.GetNearestTarget(transform.position, 100f)?.transform;
                if (_target == null)
                {
                    Debug.Log("No monster on field!!!");
                    //PlayerManager.Instance.IsAutoMoving = true;
                    _target = FindAnyObjectByType<DungeonPortalController>()?.transform;
                    // 포탈 타겟 있으면
                    if (_target != null)
                    {
                        PlayerManager.Instance.IsAutoMoving = false;
                    }
                }
                // 장거리 몬스터 타겟 있으면
                else
                {
                    PlayerManager.Instance.IsAutoMoving = false;
                }
            }
            // 근거리 몬스터 타겟 있으면
            else
            {
                PlayerManager.Instance.IsAutoMoving = false;
            }
        }
        else
        // 던전이 아닌 경우, 필드 오브젝트 찾기
        {
            _target = GameObject.FindGameObjectWithTag(Define.FieldObjectTag)?.transform;

            // 상인이면 그냥 지나가기
            if (FieldManager.Instance.CurrentEventType == Define.JourneyType.Merchant)
            {
                PlayerManager.Instance.IsAutoMoving = true;
                OnAutoMerchantAppear?.Invoke();
            }
            else
            {
                PlayerManager.Instance.IsAutoMoving = false;
            }
            //if (_target != null)
            //{
            //    if (FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
            //    {
            //        if (_target.GetComponent<Animator>().GetBool(Define.Open))
            //        {
            //            PlayerManager.Instance.IsAutoMoving = true;
            //            _target = null;
            //        }
            //        else
            //        {
            //            PlayerManager.Instance.IsAutoMoving = false;
            //        }
            //    }
            //    //
            //    else if (FieldManager.Instance.CurrentEventType == Define.JourneyType.Merchant)
            //    {
            //        PlayerManager.Instance.IsAutoMoving = true;
            //    }
            //    else if (FieldManager.Instance.CurrentEventType == Define.JourneyType.OtherObject)
            //    {
            //        PlayerManager.Instance.IsAutoMoving = false;
            //    }
            //}
        }

        if (_target == null)
            return;

        // 몬스터 타겟이거나, 보물상자 타겟일 경우
        if (_target.CompareTag(Define.MonsterTag) || FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
        {
            _targetDistance = _shortestSkillDistance;
        }
        // 포탈, 오브젝트 등 접촉해야 하는 경우엔 거리를 짧게 설정
        else
        {
            _targetDistance = 0.5f;
        }
    }

    public void ReleaseTarget()
    {
        _target = null;
    }

    public void SetAuto(bool flag)
    {
        PlayerManager.Instance.IsAuto = flag;
        _direction = Vector3.zero;
    }

    public void SetShortestSkillDistance(float distance)
    {
        _shortestSkillDistance = distance - 0.5f;
    }

    void SetPlayerCollision(bool flag)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.MonsterTag), !flag);
    }

    IEnumerator CoSetPlayerCollision(float duration)
    {
        _isSwifting = true;
        SetPlayerCollision(false);
        yield return new WaitForSeconds(duration);
        SetPlayerCollision(true);
        _isSwifting = false;
    }

    public void ProcessPlayerCollision(float duration)
    {
        StartCoroutine(CoSetPlayerCollision(duration));
    }

    public void OnOffStatusUpgrade(Define.StatusType status, float amount)
    {
        switch (status)
        {
            case Define.StatusType.Atk:
                _runtimeData.Atk += amount;
                break;

            case Define.StatusType.Def:
                _runtimeData.Def += amount;
                break;

            case Define.StatusType.HP:
                _runtimeData.HP += amount;
                break;

            case Define.StatusType.HPRecoveryPerSec:
                _runtimeData.HPRecoveryPerSec += amount;
                break;

            case Define.StatusType.CoolTimeDecrease:
                _runtimeData.CoolTimeDecrease += amount;
                break;

            default:
                break;
        }
    }

    void Die()
    {
        OnPlayerDead?.Invoke();
        _animator.SetInteger(Define.DieType, UnityEngine.Random.Range(1, 3));
        _animator.SetTrigger(Define.Die);
        Invoke("Revive", 2f);
    }

    void Revive()
    {
        HP = _playerData.HP;
        _animator.SetInteger(Define.DieType, 0);
        PlayerManager.Instance.IsAutoMoving = true;
    }
    #endregion


    #region SetBuff
    public void SetPlayerBuff()
    {
        if (_atkBuff == 0)
            _atkBuff = _runtimeData.Atk / 10;
        if (_hpBuff == 0)
            _hpBuff = _runtimeData.HP / 10;

        OnOffStatusUpgrade(Define.StatusType.Atk, _atkBuff);
        OnOffStatusUpgrade(Define.StatusType.HP, _hpBuff);
    }

    public void RemovePlayerBuff(int buffCount)
    {
        for (int i = 0; i < buffCount; i++)
        {
            OnOffStatusUpgrade(Define.StatusType.Atk, -_atkBuff);
            OnOffStatusUpgrade(Define.StatusType.HP, -_hpBuff);
        }
        _atkBuff = 0;
        _hpBuff = 0;
    }
    #endregion

    #region IDamageable Methods
    // * 방어력 적용 데미지 계산 메서드
    public void GetDamage(float damage)
    {
        if (_animator.GetInteger(Define.DieType) > 0)
            return;
        float finalDamage = CalculateFinalDamage(damage, _runtimeData.Def);
        HP -= finalDamage;
        DamageTextEvent.Invoke(Util.GetDamageTextPosition(gameObject.GetComponent<Collider>()), finalDamage, false);
        Debug.Log($"Damaged: {finalDamage}, Current Player HP: {_hp}");
        if (HP <= 0)
        {
            Die();
        }

    }

    public float CalculateFinalDamage(float damage, float def)
    {
        return damage * (1 - def / Define.MaxDef);
    }
    #endregion

    #region Equipment
    public void ApplyItemStatus(ItemStatus itemStatus)
    {
        _runtimeData.Atk += itemStatus.Atk;
        _runtimeData.Def += itemStatus.Def;
        _runtimeData.HP += itemStatus.HP;
        _runtimeData.HPRecoveryPerSec += itemStatus.HPRecoveryPerSec;
        _runtimeData.CoolTimeDecrease += itemStatus.CoolTimeDecrease;
        _runtimeData.Speed += itemStatus.Speed;
    }

    public void ReleaseItemStatus(ItemStatus itemStatus)
    {
        _runtimeData.Atk -= itemStatus.Atk;
        _runtimeData.Def -= itemStatus.Def;
        _runtimeData.HP -= itemStatus.HP;
        _runtimeData.HPRecoveryPerSec -= itemStatus.HPRecoveryPerSec;
        _runtimeData.CoolTimeDecrease -= itemStatus.CoolTimeDecrease;
        _runtimeData.Speed -= itemStatus.Speed;
    }

    public void ApplyUpgradeStatus(PlayerData playerData)
    {
        _runtimeData.Atk = playerData.Atk;
        _runtimeData.Def = playerData.Def;
        _runtimeData.HP = playerData.HP;
        _runtimeData.HPRecoveryPerSec = playerData.HPRecoveryPerSec;
    }
    #endregion
}