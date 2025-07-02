using System;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] StageInfo _stageInfo;

    private string _stageName;
    private Define.StageActionStatus _stageActionStatus;
    private bool _isSpawnNamedMonster = false;

    public Define.StageActionStatus StageActionStatus
    {
        get { return _stageActionStatus; }
        set
        {
            _stageActionStatus = value;
            OnStageActionChanged?.Invoke(_stageActionStatus);
        }
    }

    public bool IsSpawnNamedMonster
    {
        get { return _isSpawnNamedMonster; }
        set 
        { 
            _isSpawnNamedMonster = value;
            if (_isSpawnNamedMonster == true)
                FieldManager.Instance.DungeonController.OnSpawnNamedMonster?.Invoke();
        }
    }

    public event Action<Define.StageActionStatus> OnStageActionChanged;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _stageActionStatus = Define.StageActionStatus.NotChallenge;
    }

    public void OnEnable()
    {
        FieldManager.Instance.DungeonController.OnSpawnableNamedMonster += ChangeStageActionStatusToChallenge;
        FieldManager.Instance.DungeonController.OnDungeonExit += ClearSetting;
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    private void ChangeStageActionStatusToChallenge()
    {
        if (_stageActionStatus == Define.StageActionStatus.NotChallenge)
            StageActionStatus = Define.StageActionStatus.Challenge;
        else if (_stageActionStatus == Define.StageActionStatus.AutoChallenge)
            IsSpawnNamedMonster = true;
    }

    private void ClearSetting()
    {
        StageActionStatus = Define.StageActionStatus.NotChallenge;
        IsSpawnNamedMonster = false;
    }

    public void UpdateStage()
    {

    }
}
