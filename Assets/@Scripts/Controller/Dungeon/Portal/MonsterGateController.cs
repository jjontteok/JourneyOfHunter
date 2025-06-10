using UnityEngine;

public class MonsterGateController : MonoBehaviour
{
    ParticleSystem _particleSystem;

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    private void Update()
    {
        CreateGate();
    }

    void CreateGate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(8, 8, 8), Time.timeScale * 0.1f);
    }
}
