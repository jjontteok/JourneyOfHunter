using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    GameObject _player;

    Vector3 _offset = new Vector3(0f, 5f, 200f);

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        transform.position = _player.transform.position + _offset;
    }
}
