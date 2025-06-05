using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _offset = new Vector3(0f, 5f, 200f);

    void Update()
    {
        transform.position = player.position + _offset;
    }
}
