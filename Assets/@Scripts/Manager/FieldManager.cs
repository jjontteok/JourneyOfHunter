using UnityEngine;

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    PlayerController _playerController;
    float _time = 0;
    float _interval = 1.0f;

    protected override void Initialize()
    {
        _playerController = PlayerManager.Instance.Player;
    }

    void Start()
    {
        PopupUIManager.Instance.ActivateAdventureInfo();
    }
    public void Deactivate()
    {
        
    }

    public void Subscribe()
    {
        
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _interval)
        {
            _time = 0;
            _playerController.GainJourneyExp(Define.JourneyType.Default);
        }
    }
}
