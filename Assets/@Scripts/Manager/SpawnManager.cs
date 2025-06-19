using UnityEngine;
using extension;

// * SpawnManager 스크립트
//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnManager : Singleton<SpawnManager>, IEventSubscriber, IDeactivateObject
{
    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;
    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] float _monsterInterval = 2f;

    public NormalSpawnerController NormalSpawner
    {
        get { return _normalSpawner; }
    }    

    protected override void Initialize()
    {
        base.Initialize();
        GenerateNormalSpawner();
        GenerateNamedSpawner();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnDungeonEnter += SetNormalSpawnerOn;
        DungeonManager.Instance.OnSpawnNamedMonster += SetNormalSpawnerOff;
        DungeonManager.Instance.OnSpawnNamedMonster += SetNamedSpawnerOn;
        DungeonManager.Instance.OnDungeonExit += SetNamedSpawnerOff;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        _namedSpawner.gameObject.SetActive(false);
        _normalSpawner.gameObject.SetActive(false);
    }
    #endregion

    void GenerateNormalSpawner()
    {
        _normalSpawner = new GameObject("NormalMonsterSpawner").GetOrAddComponent<NormalSpawnerController>();
    }
    void GenerateNamedSpawner()
    {
        _namedSpawner = new GameObject("NamedMonsterSpawner").GetOrAddComponent<NamedSpawnerController>();
    }

    void SetNormalSpawnerOn()
    {
        _normalSpawner.gameObject.SetActive(true);
        _normalSpawner.SetSpawnerPos(DungeonManager.Instance.DungeonOffSet);
        _normalSpawner.SetSpawnerOn("Demon", _spawnInterval, _monsterInterval);
    }
    
    void SetNormalSpawnerOff()
    {
        _normalSpawner.SetSpawnerOff();
        _normalSpawner.gameObject.SetActive(false);
    }

    void SetNamedSpawnerOn()
    {
        _namedSpawner.gameObject.SetActive(true);
        _namedSpawner.SetSpawnerPos(DungeonManager.Instance.DungeonOffSet);
        _namedSpawner.SetSpawnerOn("Goblin");
    }

    void SetNamedSpawnerOff()
    {
        _namedSpawner.SetSpawnerOff();
        _namedSpawner.gameObject.SetActive(false);
    }
}
