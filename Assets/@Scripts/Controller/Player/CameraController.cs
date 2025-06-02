using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _offset = new Vector3(0f, 10f, -20f);

    private void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
    }
    void Update()
    {
        transform.position = player.position + _offset;
    }
}
