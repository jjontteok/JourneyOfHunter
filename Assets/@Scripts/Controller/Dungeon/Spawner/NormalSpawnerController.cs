using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * NormalSpawner 컨트롤러 스크립트
public class NormalSpawnerController : MonoBehaviour
{
    private List<Vector3> _spawnPosList;

    private string _monsterName;            // 스폰 될 몬스터 이름
    private WaitForSeconds _spawnInterval;  // 스폰 간격
    private float _monsterInterval = 1f;
    private Vector3 _offset;

    private Coroutine _spawnCoroutine;

    public List<Vector3> SpawnPosList
    {
        get { return _spawnPosList; }
    }

    private void OnEnable()
    {
        Initialize();
    }

    void Initialize()
    {
        _spawnPosList = new List<Vector3>();
    }

    // 스포너 위치 설정
    public void SetSpawnerPos(Vector3 offSet)
    {
        _offset = offSet;
        _spawnPosList.Add(Define.SpawnSpot1 + _offset);
        _spawnPosList.Add(Define.SpawnSpot2 + _offset);
        _spawnPosList.Add(Define.SpawnSpot3 + _offset);
        _spawnPosList.Add(Define.SpawnSpot4 + _offset);
        _spawnPosList.Add(Define.SpawnSpot5 + _offset);
        _spawnPosList.Add(Define.SpawnSpot6 + _offset);
    }

    // 스포너 설정 및 스폰 시작
    public void SetSpawnerOn(string monsterName, float spawnInterval, float monsterInterval)
    {
        _monsterName = monsterName;
        _spawnInterval = new WaitForSeconds(spawnInterval);
        _monsterInterval = monsterInterval;
        _spawnCoroutine = StartCoroutine(StartSpawn());
    }

    // 스포너 해제
    public void SetSpawnerOff()
    {
        StopCoroutine(_spawnCoroutine);
    }

    IEnumerator StartSpawn()
    {
        while(true)
        {
            SpawnMonsters();
            SpawnMonsters();
            yield return _spawnInterval;
        }
    }

    void SpawnMonsters()
    {
        int index = Random.Range(0, _spawnPosList.Count);

        GameObject monsterGate = PoolManager.Instance.GetObjectFromPool<MonsterGateController>(_spawnPosList[index] + Vector3.up * 2, "HellGate");

        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[index], _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[index] + _monsterInterval * Vector3.right, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[index] + _monsterInterval * Vector3.left, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[index] + _monsterInterval * Vector3.forward, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[index] + _monsterInterval * Vector3.back, _monsterName);
    }
}
