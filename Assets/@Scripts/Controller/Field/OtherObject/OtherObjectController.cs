using System.Collections;
using UnityEngine;

public class OtherObjectController : MonoBehaviour, IDelayAutoMoving
{
    bool _isObtained;

    private void OnEnable()
    {
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
                FieldManager.Instance.RewardSystem.GainReward(transform.position + Vector3.up);
                FieldManager.Instance.IsClear = true;
                // 1초간 정지
                StartCoroutine(CoSetAutoMoving());
            }
        }
    }
}
