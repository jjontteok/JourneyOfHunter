using UnityEngine;

public class PushColliderController : PenetrationColliderController, IGravityCollider
{
    float _radius;

    private void Awake()
    {
        _radius = GetComponent<BoxCollider>().size.x / 2;
    }

    public void ProcessGravityEffect(Collider other)
    {
        Vector3 direction = (other.transform.position - transform.position).normalized;
        direction.y = 0;
        other.GetComponent<Rigidbody>().AddForce(direction * 10000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) || other.CompareTag(Define.MonsterTag))
        {
            // 스킬 시전 후 대미지 중복 적용 방지
            if (!_damagedObjects.Contains(other.gameObject))
            {
                ProcessTrigger(other);
                ProcessGravityEffect(other);
            }
        }
    }
}
