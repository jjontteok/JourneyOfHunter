using UnityEngine;

public class DamageOverTimeColliderController : PenetrationColliderController, IDamageOverTimeCollider
{
    float _currentTime;
    const float _timeInterval = 0.5f;

    public override void SetColliderInfo(Status status, SkillData skillData)
    {
        base.SetColliderInfo(status, skillData);
        _damage = _skillData.Damage / ((int)(_skillData.DurationTime / _timeInterval) + 1);
    }

    private void OnEnable()
    {
        _currentTime = 0f;
    }

    // Unity Life Cycle 상, trigger 먼저 처리 후 update
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
        {
            TickDamage(other);
        }
    }

    private void FixedUpdate()
    {
        if (_currentTime >= _timeInterval)
        {
            _currentTime = 0f;
        }
        _currentTime += Time.fixedDeltaTime;
    }

    public virtual void TickDamage(Collider other)
    {
        if (_currentTime >= _timeInterval)
        {
            other.GetComponent<IDamageable>().GetDamage(_damage);
        }
    }
}
