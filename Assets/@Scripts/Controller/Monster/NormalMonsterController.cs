using System;
using UnityEngine;

public class NormalMonsterController : MonsterController
{
    public static Action s_OnNormalMonsterDie;

    private void Awake()
    {
        base.Initialize();
    }

    private void Update()
    {
        MoveToTarget(_target.transform.position);
    }

    public override void Die()
    {
        base.Die();
        s_OnNormalMonsterDie();
    }
}
