using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 3f;

    public bool Isturn = false;

    private Vector3 lookDir = Vector3.forward;

    void Update()
    {
        transform.position += lookDir * Time.deltaTime * speed;
        if (!Isturn && transform.position.z > 109)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
