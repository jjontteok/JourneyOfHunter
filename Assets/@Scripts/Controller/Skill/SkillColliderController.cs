using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    protected float _damage;
    protected Status _status;
    protected float _angle;
    protected GameObject _effect;
    protected ActiveSkill _connectedSkill;

    public void SetColliderInfo(float damage, Status status, GameObject connectedSkillPrefab, GameObject effect, float angle = default)
    {
        _damage = damage;
        if (connectedSkillPrefab != null)
        {
            _connectedSkill = Instantiate(connectedSkillPrefab).GetComponent<ActiveSkill>();
            _connectedSkill.Initialize(status);
            _connectedSkill.gameObject.SetActive(false);
        }
        _status = status;
        _effect = effect;
        _angle = angle;
    }

    protected void ActivateConnectedSkill()
    {
        if (_connectedSkill != null)
        {
            _connectedSkill.ActivateSkill(null, transform.position);
        }
    }

    protected virtual void InstantiateHitEffect(Collider other)
    {
        GameObject effect = Instantiate(_effect);
        effect.name = $"{_effect.name} effect";

        effect.transform.position = GetEffectPosition(other);
        Destroy(effect, 0.5f);
    }

    Vector3 GetEffectPosition(Collider other)
    {
        float height = other.GetComponent<CapsuleCollider>().height;
        height *= other.transform.lossyScale.y;
        Vector3 pos = other.transform.position;
        pos.y = height * 0.7f;
        return pos;
    }

    // 트리거 충돌 시 메서드
    protected void ProcessTrigger(Collider other)
    {
        other.GetComponent<IDamageable>().GetDamaged(_damage + _status.Atk);
        InstantiateHitEffect(other);
        //ActivateConnectedSkill();
    }
}
