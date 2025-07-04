using UnityEngine;

public class NamedSpawnerController : MonoBehaviour
{
    private Vector3 _spawnPos;

    private string _monsterName;            // 스폰 될 몬스터 이름
    GameObject _monster;

    public void SetSpawnerPos(Vector3 offset, GameObject monsterParentPool)
    {
        _spawnPos = Define.NamedMonsterSpawnSpot + offset;
    }

    public void SetSpawnerOn(string monsterName)
    {
        _monsterName = monsterName;
        SpawnNamedMonster();
    }

    private void SpawnNamedMonster()
    {
        _monster = PoolManager.Instance.GetObjectFromPool<NamedMonsterController>(_spawnPos, _monsterName, transform);
    }

    public void SetSpawnerOff()
    {
        _monster.SetActive(false);
    }
}
