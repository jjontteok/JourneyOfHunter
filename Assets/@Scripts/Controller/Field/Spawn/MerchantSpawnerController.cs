using UnityEngine;

public class MerchantSpawnerController : MonoBehaviour
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
        PoolManager.Instance.GetObjectFromPool<MerchantController>(_spawnPos, name);
    }
}
