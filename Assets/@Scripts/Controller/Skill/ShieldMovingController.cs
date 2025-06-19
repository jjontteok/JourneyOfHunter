using UnityEngine;

public class ShieldMovingController : MonoBehaviour
{
    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 3f);
    }

}
