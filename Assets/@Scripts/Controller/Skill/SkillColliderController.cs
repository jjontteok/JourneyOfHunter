using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    protected Status _status;
    protected SkillData _skillData;
    // 틱뎀 장판의 경우 GetDamaged에 넣어줄 대미지 값이 다름
    protected float _damage;

    // 2차 스킬은 객체 생성해서 갖고 있어야함
    protected ActiveSkill _connectedSkill;

    public virtual void SetColliderInfo(Status status, SkillData skillData)
    {
        _skillData = skillData;
        _status = status;
        _damage = skillData.damage;

        if (skillData.connectedSkillPrefab != null)
        {
            _connectedSkill = Instantiate(skillData.connectedSkillPrefab).GetComponent<ActiveSkill>();
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
        GameObject effect = Instantiate(_skillData.hitEffectPrefab);
        effect.name = $"{_skillData.hitEffectPrefab.name} effect";

        effect.transform.position = Util.GetEffectPosition(other);
        Destroy(effect, 0.5f);
    }

    // 트리거 충돌 시 메서드
    protected virtual void ProcessTrigger(Collider other)
    {
        other.GetComponent<IDamageable>().GetDamage(_damage);
        InstantiateHitEffect(other);
        //ActivateConnectedSkill();
    }
}
