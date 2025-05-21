using UnityEngine;

public class ParticleSkill : MonoBehaviour
{
    /// <summary>
    /// test용 - 아마도 삭제 예정
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            Debug.Log($"Particle Collision with {other.name}");
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("충돌");
    }
}
