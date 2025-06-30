using UnityEngine;
using extension;
using System.Collections.Generic;
using Unity.VisualScripting;

//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnController : MonoBehaviour
{
    Dictionary<string, GameObject> _fieldObjectSpawnSpotList;
    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;
    private TreasureBoxSpawnerController _treasureBoxSpawner;
    private MerchantSpawnerController _merchantSpawner; 

    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] float _monsterInterval = 2f;

    public NormalSpawnerController NormalSpawner
    {
        get { return _normalSpawner; }
    }    

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _fieldObjectSpawnSpotList = ObjectManager.Instance.FieldObjectSpawnSpotList;
        GenerateNormalSpawner();
        GenerateNamedSpawner();
        GenerateTreasureBoxSpawner();
        GenerateMerchantSpanwer();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    void Subscribe()
    {
        FieldManager.Instance.DungeonController.OnDungeonEnter += SetNormalSpawnerOn;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += SetNormalSpawnerOff;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += SetNamedSpawnerOn;
        FieldManager.Instance.DungeonController.OnDungeonExit += SetNamedSpawnerOff;
        FieldManager.Instance.OnJourneyEvent += SetEvent;
    }

    #region GenerateSpawner
    void GenerateNormalSpawner()
    {
        _normalSpawner = new GameObject("NormalMonsterSpawner").GetOrAddComponent<NormalSpawnerController>();
    }
    void GenerateNamedSpawner()
    {
        _namedSpawner = new GameObject("NamedMonsterSpawner").GetOrAddComponent<NamedSpawnerController>();
    }

    void GenerateTreasureBoxSpawner()
    {
        _treasureBoxSpawner = new GameObject("TreasureBoxSpawner").GetOrAddComponent<TreasureBoxSpawnerController>();
    }

    void GenerateMerchantSpanwer()
    {
        _merchantSpawner = new GameObject("MerchantSpawner").GetOrAddComponent<MerchantSpawnerController>();
    }
    #endregion


    #region MonsterSpawn Function
    void SetNormalSpawnerOn()
    {
        _normalSpawner.gameObject.SetActive(true);
        _normalSpawner.SetSpawnerPos(FieldManager.Instance.DungeonController.DungeonOffSet);
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
        _namedSpawner.SetSpawnerPos(FieldManager.Instance.DungeonController.DungeonOffSet);
        _namedSpawner.SetSpawnerOn("Goblin");
    }

    void SetNamedSpawnerOff()
    {
        _namedSpawner.SetSpawnerOff();
        _namedSpawner.gameObject.SetActive(false);
    }
    #endregion

    //FieldManager에서 이벤트 발생
    void SetEvent(Define.JourneyEventType type)
    {

    }

    void SetTreasureBoxSpawnerOn()
    {
        string spawnNumber = Random.Range(0, 4).ToString();
        _treasureBoxSpawner.gameObject.SetActive(true);
        _treasureBoxSpawner.SetSpawnerPos(_fieldObjectSpawnSpotList["FieldObjectSpawnSpot"+spawnNumber].transform.position);
        _treasureBoxSpawner.SetSpawnerOn("TreasureBox");
    }

    void SetTreasureBoxSpawnerOff()
    {
        _treasureBoxSpawner.gameObject.SetActive(false);
    }

    void SetMerchantSpanwerOn()
    {
        string spawnNumber = Random.Range(0, 4).ToString();
        _merchantSpawner.gameObject.SetActive(true);
        _merchantSpawner.SetSpawnerPos(_fieldObjectSpawnSpotList["FieldObjectSpawnSpot" + spawnNumber].transform.position);
        _merchantSpawner.SetSpawnerOn("Merchant");
    }
}
