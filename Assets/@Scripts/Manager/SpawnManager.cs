using UnityEngine;
using extension;

// * SpawnManager 스크립트
//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnManager : Singleton<SpawnManager>, IEventSubscriber, IDeactivateObject
{
    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;
    [SerializeField] float _spawnInterval = 5f;
    [SerializeField] float _monsterInterval = 4f;

    public NormalSpawnerController NormalSpawner
    {
        get { return _normalSpawner; }
    }    

    protected override void Initialize()
    {
        base.Initialize();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnDungeonEnter += SetNormalSpawnerOn;
        DungeonManager.Instance.OnSpawnNamedMonster += SetNormalSpawnerOff;
        DungeonManager.Instance.OnSpawnNamedMonster += SetNamedSpawnerOn;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        //SetSpawnerOff();
    }
    #endregion

    void SetNormalSpawnerOn()
    {
        _normalSpawner = new GameObject("NormalMonsterSpawner").GetOrAddComponent<NormalSpawnerController>();
        _normalSpawner.SetSpawnerPos();
        _normalSpawner.SetSpawnerOn("Demon", _spawnInterval, _monsterInterval);
    }
    
    void SetNormalSpawnerOff()
    {
        _normalSpawner.SetSpawnerOff();
    }

    void SetNamedSpawnerOn()
    {
        _namedSpawner = new GameObject("NamedMonsterSpawner").GetOrAddComponent<NamedSpawnerController>();
        _namedSpawner.SetSpawnerPos();
        _namedSpawner.SetSpawnerOn("Goblin");
    }
}
