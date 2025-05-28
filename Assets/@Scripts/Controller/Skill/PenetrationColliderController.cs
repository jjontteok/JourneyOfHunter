using UnityEngine;

// 충돌 시 관통 or 유지되어야 하는 스킬의 콜라이더 - TransformTargetSkill, AoENonTargetSkill
public class PenetrationColliderController : MonoBehaviour
{
    float _damage;
    GameObject _connectedSkill;
    GameObject _effect;
    ParticleSystem _particle;

    public void SetColliderInfo(float damage, GameObject connectedSkill, GameObject effect)
    {
        _damage = damage;
        _connectedSkill = connectedSkill;
        _effect = effect;
        _particle = GetComponent<ParticleSystem>();
    }

    void InstantiateConnectedSkill()
    {
        if (_connectedSkill != null)
        {
            GameObject connectedSkill = Instantiate(_connectedSkill);
            connectedSkill.transform.position = transform.position;
        }            
    }

    void InstantiateHitEffect(Collider other)
    {
        GameObject effect = Instantiate(_effect);
        effect.name = $"{_effect.name} effect";

        effect.transform.position = GetEffectPosition(other);

        if (_particle != null)
        {
            _particle.Play();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            //other.GetComponent<PlayerController>().GetDamaged(_damage);
            
            InstantiateHitEffect(other);
            //InstantiateConnectedSkill();
        }
        if (other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage);

            InstantiateHitEffect(other);
            InstantiateConnectedSkill();
        }
        // Activate on Ground
        if (other.CompareTag(Define.GroundTag))
        {
            InstantiateConnectedSkill();
        }
    }
}
