using System;
using UnityEngine;

public class NormalMonsterController : MonsterController
{
    public static Action s_OnNormalMonsterDie;
    public static int s_AliveCount = 0;

    private void Awake()
    {
        base.Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        s_AliveCount++;
    }

    private void OnDisable()
    {
        s_AliveCount--;
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
