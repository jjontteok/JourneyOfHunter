using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Transform _player;

    Vector3 _offset = new Vector3(0f, 5f, 200f);

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player = PlayerManager.Instance.Player.transform;
    }

    void Update()
    {
        transform.position = _player.position + _offset;
    }
}
