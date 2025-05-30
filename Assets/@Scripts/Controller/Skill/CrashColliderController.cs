using UnityEngine;

// 충돌 시 사라져야 하는 스킬 오브젝트의 콜라이더 - RigidbodyTargetSkill
public class CrashColliderController : MonoBehaviour
{
    float _damage;
    float _atk;
    GameObject _effect;
    ActiveSkill _connectedSkill;
    ParticleSystem _particle;

    public void SetColliderInfo(float damage, float atk, GameObject connectedSkillPrefab, GameObject effect)
    {
        _damage = damage;
        if (connectedSkillPrefab != null)
        {
            _connectedSkill=Instantiate(connectedSkillPrefab).GetComponent<ActiveSkill>();
            _connectedSkill.gameObject.SetActive(false);
        }
        _atk = atk;
        _effect = effect;
        _particle = GetComponent<ParticleSystem>();
    }

    void ActivateConnectedSkill()
    {
        if (_connectedSkill != null)
        {
            _connectedSkill.ActivateSkill(null, transform.position);
            // 부모 스킬 꺼주기
            transform.parent.gameObject.SetActive(false);
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
        transform.parent.gameObject.SetActive(false);
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
            other.GetComponent<PlayerController>().GetDamaged(_damage * _atk * _atk);

            InstantiateHitEffect(other);
            //ActivateConnectedSkill();
        }
        if (other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage * _atk * _atk);

            InstantiateHitEffect(other);
            ActivateConnectedSkill();
        }
        // Activate when collide with Ground
        if (other.CompareTag(Define.GroundTag))
        {
            ActivateConnectedSkill();
        }
    }
}
