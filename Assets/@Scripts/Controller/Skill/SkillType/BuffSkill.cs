using UnityEngine;

public class BuffSkill : Skill
{
    public override bool ActivateSkill(Vector3 pos)
    {
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
    }
}
