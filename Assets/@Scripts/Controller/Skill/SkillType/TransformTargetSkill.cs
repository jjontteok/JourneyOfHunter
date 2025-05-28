using UnityEngine;

public class TransformTargetSkill : TargetSkill
{
    PenetrationColliderController _coll;
    bool _isCasting = false;
    float _currentTime = 0f;
    Vector3 dir;

    void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (_skillData.castingTime > 0)
        {
            _isCasting = true;
            //dir = _direction;
        }
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<PenetrationColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    private void Update()
    {
        // 캐스팅 동작 중이지 않을 땐 distance까지 이동
        if (!_isCasting)
        {
            if (Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
            {
                _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _skillData.castingTime)
            {
                _isCasting = false;
                _currentTime = 0f;
            }
        }
    }
}
