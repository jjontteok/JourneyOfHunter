using extension;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController _player;
    private SkillSystem _skillSystem;

    private bool _isAuto = false;
    private bool _isAutoMoving = false;

    readonly Vector3 _originPos = new Vector3(0f, 0.1f, 0.5f);

    #region Properties

    public SkillSystem SkillSystem { get { return _skillSystem; } }

    public bool IsAuto
    {
        get { return _isAuto; }
        set
        {
            _isAuto = value;
            _skillSystem.IsAuto = value;
            // 자동 모드 꺼지면 타겟도 초기화
            if (!value)
            {
                _player.Target = null;
                _isAutoMoving = false;
            }
            // 던전 생성 버튼이 활성화되어있는데 자동 모드 켜질때
            else
            {
                if (!DungeonManager.Instance.IsDungeonExist && StageManager.Instance.StageActionStatus == Define.StageActionStatus.NotChallenge)
                {
                    _player.OnAutoTeleport?.Invoke();
                }
            }
        }
    }

    public bool IsAutoMoving
    {
        get { return _isAutoMoving; }
        set { _isAutoMoving = value; Debug.Log("IsAutoMoving: " + _isAutoMoving); }
    }

    public PlayerController Player
    {
        get { return _player; }
    }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        _player = Instantiate(ObjectManager.Instance.PlayerResource, _originPos, Quaternion.identity).GetComponent<PlayerController>();
        _skillSystem = _player.GetOrAddComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();
    }
}
