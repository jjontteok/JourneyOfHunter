using UnityEngine;

public class ParticleSkill : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            Debug.Log($"Particle Collision with {other.name}");
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Ãæµ¹");
    }
}
