using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            PopupUIManager.Instance.ActivateMerchantPanel();
            // 말걸면 가만히 서있게
            PlayerManager.Instance.IsAutoMoving = false;
            FieldManager.Instance.IsClear = true;
        }
    }
}
