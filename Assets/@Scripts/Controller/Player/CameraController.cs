using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _offset = new Vector3(0f, 6f, -20f);

    private void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(10, 0, 0));
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(5, 0, 0));
        transform.position = player.position + _offset;
    }
}
