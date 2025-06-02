using System.Collections.Generic;
using UnityEngine;

// 충돌 시 관통 or 유지되어야 하는 스킬의 콜라이더 - TransformTargetSkill, AoENonTargetSkill
public class PenetrationColliderController : MonoBehaviour
{
    float _damage;
    float _atk;
    float _angle;
    GameObject _effect;
    ActiveSkill _connectedSkill;
    ParticleSystem _particle;
    HashSet<GameObject> _damagedObjects = new HashSet<GameObject>();

    public void SetColliderInfo(float damage, float atk, GameObject connectedSkillPrefab, GameObject effect, float angle = default)
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
        _angle = angle;
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

        //if (_particle != null)
        //{
        //    _particle.Play();
        //}
        Destroy(effect, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_damagedObjects.Contains(other.gameObject))
        {
            if (other.CompareTag(Define.PlayerTag))
            {
                // angle 값이 없거나, 있는데 콜라이더가 각도 내에 있을 경우 대미지
                if (_angle == 0f || (_angle > 0 && IsColliderInRange(other)))
                {
                    _damagedObjects.Add(other.gameObject);
                    other.GetComponent<PlayerController>().GetDamaged(_damage);

                    InstantiateHitEffect(other);
                    //ActivateConnectedSkill();
                    Vector3 pos = other.transform.position + new Vector3(0, other.bounds.center.y * 2, 0);
                    DamageTextEvent.Invoke(pos, _damage, false);
                }
            }
            if (other.CompareTag(Define.MonsterTag))
            {
                if (_angle == 0f || (_angle > 0 && IsColliderInRange(other)))
                {
                    _damagedObjects.Add(other.gameObject);
                    other.GetComponent<MonsterController>().GetDamaged(_damage);

                    InstantiateHitEffect(other);
                    ActivateConnectedSkill();
                    Vector3 pos = other.transform.position + new Vector3(0, other.bounds.center.y * 2, 0);
                    DamageTextEvent.Invoke(pos, _damage, false);
                }                   
            }
        }
        
        // Activate on Ground
        //if (other.CompareTag(Define.GroundTag))
        //{
        //    ActivateConnectedSkill();
        //}
    }

    // 콜라이더가 부채꼴 내에 있는지 판별
    bool IsColliderInRange(Collider collider)
    {
        Vector3 toMonster = (collider.transform.position - transform.position).normalized;
        float degree = GetAngleBetweenDirections(toMonster, transform.forward);
        if (degree <= _angle / 2)
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

    // Get angle between two normalized vectors
    float GetAngleBetweenDirections(Vector3 from, Vector3 to)
    {
        float dot = Vector3.Dot(from, to);
        float degree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return degree;
    }

    private void OnDisable()
    {
        _damagedObjects.Clear();
    }
}
