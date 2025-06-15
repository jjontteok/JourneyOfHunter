using UnityEngine;

public class DamageOverTimeColliderController : PenetrationColliderController, IDamageOverTimeCollider
{
    float _currentTime;
    const float _timeInterval = 0.5f;

    public override void SetColliderInfo(Status status, SkillData skillData)
    {
        base.SetColliderInfo(status, skillData);
        _damage = _skillData.damage / ((int)(_skillData.durationTime / _timeInterval) + 1);
    }

    private void OnEnable()
    {
        _currentTime = 0f;
    }

    // Unity Life Cycle 상, trigger 먼저 처리 후 update
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
        {
            if (_currentTime >= _timeInterval)
            {
                TickDamage(other);
            }
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
        other.GetComponent<IDamageable>().GetDamaged(_damage);
    }
}
