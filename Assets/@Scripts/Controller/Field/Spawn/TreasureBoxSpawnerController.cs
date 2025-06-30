using UnityEngine;

public class TreasureBoxSpawnerController : MonoBehaviour
{
    Vector3 _spawnPos;
    string _fieldObjectName;

    public void SetSpawnerPos(Vector3 offset)
    {
        _spawnPos = offset;

    }

    public void SetSpawnerOn(string name)
    {
        _fieldObjectName = name;
        SpawnFieldObject();
    }

    void SpawnFieldObject()
    {
        PoolManager.Instance.GetObjectFromPool<TreasureBoxController>(_spawnPos, name);
    }
}
