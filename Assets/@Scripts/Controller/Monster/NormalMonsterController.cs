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

    protected override void UpgradeStatus(float amount)
    {
        _runtimeData.Atk *= amount;
        _runtimeData.Def *= amount;
        _runtimeData.MaxHP *= amount;
        _runtimeData.CurrentHP *= amount;

        if (_runtimeData.Atk != _monsterData.Atk)
        {
            _monsterData.Atk = _runtimeData.Atk;
            _monsterData.Def = _runtimeData.Def;
            _monsterData.HP = _runtimeData.MaxHP;
        }
    }

    public override void Die()
    {
        base.Die();
        s_OnNormalMonsterDie();
    }
}
