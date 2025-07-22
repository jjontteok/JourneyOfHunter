using System;
using UnityEngine;

public class NormalMonsterController : MonsterController
{
    [SerializeField] WeaponColliderController _weapon;
    public static Action s_OnNormalMonsterDie;
    public static int s_AliveCount = 0;

    bool _isFlashed = false;

    public bool IsFlashed
    {
        get; set;
    }

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
        _isFlashed = false;
        s_AliveCount--;
    }

    private void Update()
    {
        if(!_isFlashed)
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
