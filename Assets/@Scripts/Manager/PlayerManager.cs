using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController _player;

    public PlayerController Player
    {
        get { return _player; }
    }

    protected override void Initialize()
    {
        base.Initialize();
        _player = GameObject.FindGameObjectWithTag(Define.PlayerTag).GetComponent<PlayerController>();
    }
}
