using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    protected Status _status;
    protected SkillData _skillData;
    // 틱뎀 장판의 경우 GetDamaged에 넣어줄 대미지 값이 다름
    protected float _damage;

    // 2차 스킬은 객체 생성해서 갖고 있어야함
    protected ActiveSkill _connectedSkill;
    protected GameObject _hitEffect;

    public virtual void SetColliderInfo(Status status, SkillData skillData)
    {
        _skillData = skillData;
        _status = status;
        _damage = skillData.Damage;
        _hitEffect = skillData.HitEffectPrefab;

        if (skillData.ConnectedSkillPrefab != null)
        {
            _connectedSkill = Instantiate(skillData.ConnectedSkillPrefab).GetComponent<ActiveSkill>();
            _connectedSkill.Initialize(status);
            _connectedSkill.gameObject.SetActive(false);
        }
    }

    protected void ActivateConnectedSkill()
    {
        if (_connectedSkill != null)
        {
            _connectedSkill.ActivateSkill(transform.position);
        }
    }

    protected virtual void InstantiateHitEffect(Collider other)
    {
        if (_hitEffect!=null)
        {
            GameObject effect = Instantiate(_hitEffect);
            effect.name = $"{_hitEffect.name} effect";

            effect.transform.position = Util.GetEffectPosition(other);
            Destroy(effect, 0.5f);
        }        
    }

    // 트리거 충돌 시 메서드
    protected virtual void ProcessTrigger(Collider other)
    {
        other.GetComponent<IDamageable>().GetDamage(Util.GetEnhancedDamage(_damage, _skillData));
        InstantiateHitEffect(other);
        //ActivateConnectedSkill();
    }

    public void OnOffHitEffect(bool flag)
    {
        _hitEffect = flag ? _skillData.HitEffectPrefab : null;
    }
}
