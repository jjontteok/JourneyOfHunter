using System.Collections;
using UnityEngine;

public class DirectionNonTargetSkill : NonTargetSkill
{
    // 방향? = 플레이어 정면
    Vector3 _direction;
    bool _isCasting = false;
    float _currentTime = 0f;
    // 스킬 각도
    float _angle = 120f;
    ParticleSystem _particle;

    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if(_isCasting)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _skillData.castingTime)
            {
                _isCasting = false;
                _currentTime = 0f;
                ActivateDNTSkill(transform.position);
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        _particle = GetComponentInChildren<ParticleSystem>();
    }

    // DirectionNonTargetSkill에서는 이게 진짜 activate의 역할
    void ActivateDNTSkill(Vector3 pos = default)
    {        
        base.ActivateSkill(null, pos);
        // 현재 플레이어가 바라보는 방향 == 스킬 발동 방향
        //_direction = (transform.localRotation * transform.parent.rotation).eulerAngles;
        _direction = FindAnyObjectByType<PlayerController>().transform.forward;
        Quaternion quaternion = Quaternion.FromToRotation(transform.forward, _direction);
        transform.localRotation *= quaternion;
        //transform.rotation = Quaternion.Euler(_direction);
        // 발동 방향으로 _range 각도와 targetDistance 거리 내의 적 탐색
        Collider[] targets = Physics.OverlapSphere(transform.position, _skillData.targetDistance, 1 << LayerMask.NameToLayer(Define.MonsterTag));
        foreach (Collider collider in targets)
        {
            Debug.Log(collider.name);
            if (IsColliderInRange(collider))
            {
                collider.GetComponent<MonsterController>().GetDamaged(_skillData.damage);
                GameObject effect = Instantiate(_skillData.hitEffectPrefab, GetEffectPosition(collider), Quaternion.identity);
                Destroy(effect, 0.5f);
            }
        }
    }

    // 스킬 발동 순간, 그 앞의 범위 내에 있는 적들에게 대미지
    public override void ActivateSkill(Transform target = null, Vector3 pos = default)
    {
        //1단 스킬을 켜
        base.ActivateSkill(null, pos);
        //2제 파티클 꺼주고 castingTime을 기다려
        _particle.Stop();

        _isCasting = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _skillData.targetDistance);
        Vector3 angle1 = new Vector3(Mathf.Sin((-_angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, (Mathf.Cos((-_angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad)));
        Vector3 angle2 = new Vector3(Mathf.Sin((_angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, (Mathf.Cos((_angle / 2f + transform.eulerAngles.y) * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, transform.position + angle1 * _skillData.targetDistance);
        Gizmos.DrawLine(transform.position, transform.position + angle2 * _skillData.targetDistance);
    }

    // 콜라이더가 부채꼴 내에 있는지 판별
    bool IsColliderInRange(Collider collider)
    {
        Vector3 toMonster = (collider.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(toMonster, _direction);
        float degree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (Mathf.Abs(degree) <= _angle / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 GetEffectPosition(Collider other)
    {
        float height = other.GetComponent<CapsuleCollider>().height;
        height *= other.transform.lossyScale.y;
        Vector3 pos = other.transform.position;
        pos.y = height * 0.7f;
        return pos;
    }
}
