using UnityEngine;

public class ActiveSkill : Skill
{
    protected Animator animator;

    protected override void ActivateSkill(Transform target) { }

    void Start()
    {
        animator = _player.GetComponent<Animator>();
    }
}
