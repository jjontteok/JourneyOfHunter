using UnityEngine;

public class WeaponColliderController : MonoBehaviour
{
    float _damage;
    GameObject _effect;

    public void SetColliderInfo(float damage, GameObject effect)
    {
        _damage = damage;
        _effect = effect;
    }

    void InstantiateHitEffect(Collider other)
    {
        GameObject effect = Instantiate(_effect);
        effect.name = $"{_effect.name} effect";

        effect.transform.position = GetEffectPosition(other);

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
        // 상호작용 가능 시간 확인 변수 필요할 수도
        if (other.CompareTag(Define.PlayerTag))
        {
            //other.GetComponent<PlayerController>().GetDamaged(_damage);

            InstantiateHitEffect(other);
        }
        if (other.CompareTag(Define.MonsterTag))
        {
           other.GetComponent<MonsterController>().GetDamage(_damage);
            InstantiateHitEffect(other);
        }
    }
}
