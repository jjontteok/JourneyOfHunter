using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    float _speed;
    float _damage;
    Vector3 _direction;

    public void SetColliderInfo(float speed, float damage)
    {
        _speed = speed;
        _damage = damage;
    }

    public void SetColliderDirection(Vector3 dir)
    {
        _direction = dir;
    }

    void Update()
    {
        transform.Translate(_direction.normalized * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage);
        }
    }
}
