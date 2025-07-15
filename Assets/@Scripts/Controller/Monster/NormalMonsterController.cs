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

    private void OnEnable()
    {
        s_AliveCount++;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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
