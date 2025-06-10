using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * NormalSpawner 컨트롤러 스크립트
public class NormalSpawnerController : MonoBehaviour
{
    private List<Vector3> _spawnPosList;

    private string _monsterName;            // 스폰 될 몬스터 이름
    private WaitForSeconds _spawnInterval;  // 스폰 간격

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
    public void SetSpawnerPos()
    {
        _spawnPosList.Add(Define.SpawnSpot1);
        _spawnPosList.Add(Define.SpawnSpot2);
        _spawnPosList.Add(Define.SpawnSpot3);
        _spawnPosList.Add(Define.SpawnSpot4);
        _spawnPosList.Add(Define.SpawnSpot5);
        //_spawnPosList.Add(Define.SpawnSpot6);
    }

    // 스포너 설정 및 스폰 시작
    public void SetSpawnerOn(string monsterName, float spawnInterval)
    {
        _monsterName = monsterName;
        _spawnInterval = new WaitForSeconds(spawnInterval);
        StartCoroutine(StartSpawn());
    }

    // 스포너 해제
    public void SetSpawnerOff()
    {
        StopCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        while(true)
        {
            SpawnMonsters();
            yield return _spawnInterval;
        }
    }

    void SpawnMonsters()
    {
        for(int i=0; i<_spawnPosList.Count; i++)
        {
            PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[i], _monsterName);
            PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[i] + Vector3.right, _monsterName);
            PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[i] + Vector3.left, _monsterName);
            PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[i] + Vector3.forward, _monsterName);
            PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(_spawnPosList[i] + Vector3.back, _monsterName);
        }
    }
}
