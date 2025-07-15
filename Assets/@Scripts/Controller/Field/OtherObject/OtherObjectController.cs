using UnityEngine;

public class OtherObjectController : MonoBehaviour
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            if (!_isObtained)
            {
                _isObtained = true;
                _audioSource.Stop();
                FieldManager.Instance.RewardSystem.GainReward(transform.position + Vector3.up);
                PlayerManager.Instance.IsAutoMoving = true;
            }
        }
    }
}
