using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    float _speed;
    float _damage;
    float _distance;
    float _castingTime;

    float _currentTime = 0f;
    bool _isCasting = false;
    Vector3 _direction;
    GameObject _effect;

    public void SetColliderInfo(float speed, float damage, float distance, float casting, GameObject effect)
    {
        _speed = speed;
        _damage = damage;
        _distance = distance;
        _castingTime = casting;
        _effect = effect;
    }

    public void SetColliderDirection(Vector3 dir)
    {
        _direction = dir;
    }

    // activate되면(활성화되면), 캐스팅 동작 시작
    private void OnEnable()
    {
        if (_castingTime > 0)
        {
            _isCasting = true;
        }
    }

    void Update()
    {
        // 캐스팅 동작 중이지 않을 땐 distance까지 이동
        if (!_isCasting)
        {
            if (Vector3.Distance(transform.position, transform.parent.position) < _distance)
            {
                transform.Translate(_direction.normalized * _speed * Time.deltaTime);
                //InfiniteLoopDetector.Run();
            }
        }
        // 캐스팅 동작 중일 때
        else
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _castingTime)
            {
                _isCasting = false;
                _currentTime = 0f;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage);
            // raycast를 이용해 스킬의 충돌 지점 계산
            RaycastHit hit;
            Vector3 direction = (other.transform.position - transform.position).normalized;
            Physics.Raycast(transform.position, direction, out hit);
            // 충돌 지점에서 반대 방향으로 hit effect 발생
            GameObject effect = Instantiate(_effect, hit.point, Quaternion.Inverse(Quaternion.Euler(direction)));
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.5f);
        }
    }
}
