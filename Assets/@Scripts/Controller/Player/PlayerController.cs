using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Collider _collider;
    public Vector3 Center
    {
        get => _collider.bounds.center;
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }
}
