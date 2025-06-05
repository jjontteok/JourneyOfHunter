using UnityEngine;
using extension;

// * SpawnManager 스크립트
//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnManager : Singleton<SpawnManager>, IEventSubscriber, IDeactivateObject
{
    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;

    protected override void Initialize()
    {
        base.Initialize();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnDungeonEnter += SetSpawner;
        DungeonManager.Instance.OnDungeonExit += SetSpawnerOff;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        //SetSpawnerOff();
    }
    #endregion

    void SetSpawner()
    {
        _normalSpawner = new GameObject("NormalMonsterSpawner").GetOrAddComponent<NormalSpawnerController>();
        _normalSpawner.SetSpawnerPos();
        _normalSpawner.SetSpawnerOn("Demon", 5.0f);
    }
    
    void SetSpawnerOff()
    {
        _normalSpawner.SetSpawnerOff();
    }

}
