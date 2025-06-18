using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;

    Vector3 _offset = new Vector3(0f, 6f, -20f);

    private void Start()
    {
        player = PlayerManager.Instance.Player.transform;
        transform.rotation = Quaternion.Euler(new Vector3(10, 0, 0));
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(5, 0, 0));
        transform.position = player.position + _offset;
    }
}
