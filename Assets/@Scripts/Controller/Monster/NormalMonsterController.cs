using System;
using UnityEngine;

public class NormalMonsterController : MonsterController
{
    [SerializeField] WeaponColliderController _weapon;
    public static Action s_OnNormalMonsterDie;
    public static int s_AliveCount = 0;

    private void Awake()
    {
        base.Initialize();
        _weapon.SetColliderInfo(_runtimeData.Atk);
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

    protected override void UpgradeStatus(int stage)
    {
        _runtimeData.Atk *= 2;
        _runtimeData.Def *= 2;
        _runtimeData.MaxHP *= 2;
    }

    public override void Die()
    {
        base.Die();
        s_OnNormalMonsterDie();
    }
}
