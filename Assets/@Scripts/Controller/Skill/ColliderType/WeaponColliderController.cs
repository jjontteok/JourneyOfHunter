using UnityEngine;

public class WeaponColliderController : MonoBehaviour
{
    float _damage;
    Animator _animator;
    [SerializeField] GameObject _hitEffect;

    public void SetColliderInfo(float damage)
    {
        _damage = damage;
        _animator = GetComponentInParent<Animator>();
    }

    void InstantiateHitEffect(Collider other)
    {
        GameObject effect = Instantiate(_hitEffect);
        effect.name = $"{_hitEffect.name} effect";

        effect.transform.position = Util.GetEffectPosition(other);

        Destroy(effect, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
        {
            // 상호작용 가능 시간 확인 변수
            if (_animator.GetBool(Define.IsInteractionPossible))
            {
                other.GetComponent<IDamageable>().GetDamage(_damage);
                InstantiateHitEffect(other);
            }
        }
    }
}
