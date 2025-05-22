using UnityEngine;

public class NonTargetSkill : ActiveSkill
{
    // NonTarget이므로 target 받을 필욘 없ㅇ
    protected override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(null);
        //gameObject.SetActive(true);
        //gameObject.GetComponent<ParticleSystem>()?.Play();
        //StartCoroutine(DeActivateSkill());
    }
}
