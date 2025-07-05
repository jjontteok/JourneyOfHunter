using extension;
using UnityEngine;

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
            // 자동 모드 꺼지면 타겟도 초기화
            // AutoChallenge 중이면 NotChallenge로 변화
            if (!value)
            {
                _player.Target = null;
                _isAutoMoving = false;
                if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.AutoChallenge)
                {
                    FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.NotChallenge;
                }
            }
            else
            {
                if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.NotChallenge)
                {
                    // 던전 생성 버튼이 활성화되어있는데 자동 모드 켜질때
                    if (!FieldManager.Instance.DungeonController.IsDungeonExist)
                    {
                        _player.OnAutoDungeonChallenge?.Invoke();
                    }
                    // 던전 진행 중이고, 아직 게이지 다 안 찬 NotChallenge상태에서 자동 켜질 때
                    else if (FieldManager.Instance.DungeonController.IsDungeonExist)
                    {
                        FieldManager.Instance.StageController.StageActionStatus = Define.StageActionStatus.AutoChallenge;
                    }
                }
                else if (FieldManager.Instance.StageController.StageActionStatus == Define.StageActionStatus.Challenge)
                {
                    //StageController.Instance.StageActionStatus = Define.StageActionStatus.ExitStage;
                    FieldManager.Instance.StageController.IsSpawnNamedMonster = true;
                }
            }
            //Debug.Log("IsAuto: " + _isAuto);
        }
    }

    // 하나의 필드 내에서 이벤트가 끝나고 다음 필드로 향할 때
    // 던전 => 포탈을 향해 가야할 때(target==portal), 몬스터랑 포탈 다 없는 잠깐의 순간(target==null)
    // 오브젝트 => 상호작용을 통해 보상을 얻고 난 후
    public bool IsAutoMoving
    {
        get { return _isAutoMoving; }
        set
        {
            if (_isAutoMoving != value)
            {
                _isAutoMoving = value;
            }
        }
    }

    public bool IsDead { get; set; }

    public PlayerController Player
    {
        get { return _player; }
    }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        _player = Instantiate(ObjectManager.Instance.PlayerResource, _originPos, Quaternion.identity).GetComponent<PlayerController>();
        _skillSystem = _player.gameObject.GetOrAddComponent<SkillSystem>();
        _skillSystem.InitializeSkillSystem();
    }
}
