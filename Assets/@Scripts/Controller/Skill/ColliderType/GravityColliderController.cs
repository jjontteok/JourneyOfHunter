using System.Collections;
using UnityEngine;

public class GravityColliderController : DamageOverTimeColliderController, IGravityCollider
{
    [SerializeField] GameObject _gravityObject;
    [SerializeField] GameObject _lastExplosion;

    float _worldRadius;
    const float _maxForce = 50f;
    const float _minForce = 10f;

    private void Awake()
    {
        _worldRadius = GetComponent<CapsuleCollider>().radius * transform.lossyScale.x;
    }

    private void OnEnable()
    {
        _gravityObject.SetActive(true);
        _gravityObject.transform.localScale = Vector3.one * 10f;
        transform.localScale = Vector3.one * 10f;
        _lastExplosion.SetActive(false);
        StartCoroutine(CoControlScale());
    }

    public void ProcessGravityEffect(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            Vector3 difference = _gravityObject.transform.position - other.transform.position;
            float distance = difference.magnitude;
            difference.y *= 0.5f;
            Vector3 direction = difference.normalized;
            float forceStrength = Mathf.Lerp(_minForce, _maxForce, distance / _worldRadius);
            rb.AddForce(direction * forceStrength);
        }
    }

    public override void TickDamage(Collider other)
    {
        base.TickDamage(other);
        ProcessGravityEffect(other);
    }

    IEnumerator CoControlScale()
    {
        Vector3 targetScale = Vector3.one;
        while (true)
        {
            yield return null;
            _gravityObject.transform.localScale = Vector3.Lerp(_gravityObject.transform.localScale, targetScale, Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
            if (Vector3.Distance(_gravityObject.transform.localScale, targetScale) <= 0.5f)
            {
                _gravityObject.SetActive(false);
                //폭발 이펙트 On
                _lastExplosion.SetActive(true);
                //_lastExplosion.transform.localScale = Vector3.one * 0.2f;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _worldRadius);
    }
}
