using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class RedSlash : TargetSkill, SwordMotion
{
    Vector3 _euler = new Vector3(0, 0, 90f);
    Vector3 pos;
    public void SwordAttack()
    {
        _animator.SetTrigger(Define.Attack);
        _animator.SetBool(Define.IsAttacking, true);
    }

    protected override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(target);
        transform.localRotation = Quaternion.Euler(_euler);
        _coll.transform.localScale = Vector3.zero;
        _coll.SetColliderDirection(Vector3.forward);
        SwordAttack();
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Ãæµ¹!");
        //EditorApplication.isPaused = true;
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
        ParticleSystem ps = GetComponent<ParticleSystem>();
        
        int num = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        foreach(var particle in particles)
        {
            pos = particle.position;
            //Vector3 worldPos = ps.transform.TransformPoint(pos);
            //pos += _player.transform.position;
            //pos = worldPos;
            //Debug.Log(pos);
            Collider[] colls = Physics.OverlapSphere(pos, 1f, 1 << LayerMask.NameToLayer(Define.MonsterTag));
            foreach (Collider coll in colls)
            {
                coll.GetComponent<MonsterController>().GetDamaged(10);
            }
        }
    }
}
