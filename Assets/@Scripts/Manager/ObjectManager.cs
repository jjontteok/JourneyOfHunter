using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private GameObject _demonResource;

    public void ResourceLoad()
    {
        _demonResource = Resources.Load<GameObject>(Define.DemonPath);
    }
}
