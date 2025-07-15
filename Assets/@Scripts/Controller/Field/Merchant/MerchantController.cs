using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            PopupUIManager.Instance.ActivateMerchantPanel();
        }
    }
}
