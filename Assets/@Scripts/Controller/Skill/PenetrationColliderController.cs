using System.Collections.Generic;
using UnityEngine;

// 충돌 시 관통 or 유지되어야 하는 스킬의 콜라이더 - TransformTargetSkill, AoENonTargetSkill
public class PenetrationColliderController : MonoBehaviour
{
    float _damage;
    float _atk;
    GameObject _effect;
    ActiveSkill _connectedSkill;
    ParticleSystem _particle;
    HashSet<GameObject> _damagedObjects = new HashSet<GameObject>();

    public void SetColliderInfo(float damage, float atk, GameObject connectedSkillPrefab, GameObject effect)
    {
        _damage = damage;
        if (connectedSkillPrefab != null)
        {
            _connectedSkill = Instantiate(connectedSkillPrefab).GetComponent<ActiveSkill>();
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
        if(!_damagedObjects.Contains(other.gameObject))
        {
            if (other.CompareTag(Define.PlayerTag))
            {
                _damagedObjects.Add(other.gameObject);
                other.GetComponent<PlayerController>().GetDamaged(_damage * _atk * _atk);

                InstantiateHitEffect(other);
                //ActivateConnectedSkill();
            }
            if (other.CompareTag(Define.MonsterTag))
            {
                _damagedObjects.Add(other.gameObject);
                other.GetComponent<MonsterController>().GetDamaged(_damage * _atk * _atk);

                InstantiateHitEffect(other);
                ActivateConnectedSkill();
            }
        }
        
        // Activate on Ground
        //if (other.CompareTag(Define.GroundTag))
        //{
        //    ActivateConnectedSkill();
        //}
    }

    private void OnDisable()
    {
        _damagedObjects.Clear();
    }
}
