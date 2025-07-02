using UnityEngine;

public class OtherObjectController : MonoBehaviour
{
    bool _isObtained;

    private void OnEnable()
    {
        _isObtained = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            if (!_isObtained)
            {
                _isObtained = true;
                TextManager.Instance.ActivateRewardText(transform.position + Vector3.up, "여정의 증표", 5);
            }
        }
    }
}
