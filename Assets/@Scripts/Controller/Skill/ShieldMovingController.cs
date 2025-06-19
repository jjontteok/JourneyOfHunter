using UnityEngine;

public class ShieldMovingController : MonoBehaviour
{
    float _speed = 5f;
    Vector3[] _direction = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    [SerializeField] GameObject[] _shields;

    private void OnEnable()
    {
        //for (int i = 0; i < _direction.Length; i++)
        //{
        //    _shields[i].transform.localPosition = _direction[i];
        //}
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 3f);
    }

}
