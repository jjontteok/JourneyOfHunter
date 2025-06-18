using UnityEngine;

public class GravityColliderController : DamageOverTimeColliderController, IGravityCollider
{
    float _worldRadius;

    private void Awake()
    {
        _worldRadius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }

    public void ProcessGravityEffect(Collider other)
    {
        Vector3 direction = (transform.position - other.transform.position).normalized;
        // 틱 대미지에 끌어당기는 효과도 추가
        other.GetComponent<Rigidbody>().AddForce(direction * 7f,ForceMode.Impulse);
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
