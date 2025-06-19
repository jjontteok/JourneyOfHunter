using System.Collections;
using UnityEngine;

public class PassiveSkill : Skill
{
    public override bool ActivateSkill(Vector3 pos)
    {
        return true;
    }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
    }

    protected override IEnumerator DeActivateSkill()
    {
        throw new System.NotImplementedException();
    }
}
