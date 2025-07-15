using System.Collections;
using UnityEngine;

public class OtherObjectController : MonoBehaviour, IDelayAutoMoving
{
    bool _isObtained;
    AudioSource _audioSource;
    GameObject _player;

    float _distance;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = PlayerManager.Instance.Player.gameObject;
    }

    private void OnEnable()
    {
        _audioSource.Play();
        _isObtained = false;
    }

    public IEnumerator CoSetAutoMoving()
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance.IsAutoMoving = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            if (!_isObtained)
            {
                _isObtained = true;
                _audioSource.Stop();
                FieldManager.Instance.RewardSystem.GainReward(transform.position + Vector3.up);
                FieldManager.Instance.IsClear = true;
                // 1초간 정지
                StartCoroutine(CoSetAutoMoving());
            }
        }
    }
}
