using UnityEngine;

public class NonTargetSkill : Skill
{
    protected override void ActivateSkill(Transform target)
    {
        transform.position = _player.transform.position;
        gameObject.SetActive(true);
        StartCoroutine(DeActivateSkill());
    }
}
