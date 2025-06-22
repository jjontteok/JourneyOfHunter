using UnityEngine;

public class GravityColliderController : DamageOverTimeColliderController, IGravityCollider
{
    float _worldRadius;
    const float _maxForce = 20f;
    const float _minForce = 10f;

    private void Awake()
    {
        _worldRadius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }

    public void ProcessGravityEffect(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            Vector3 difference = transform.position - other.transform.position;
            Vector3 direction = difference.normalized;
            float distance = difference.magnitude;
            float forceStrength = Mathf.Lerp(_minForce, _maxForce, distance / _worldRadius);
            rb.AddForce(direction * forceStrength);
        }
    }

    public override void TickDamage(Collider other)
    {
        base.TickDamage(other);
        ProcessGravityEffect(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _worldRadius);
    }
}
