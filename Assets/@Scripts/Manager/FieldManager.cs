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
        PopupUIManager.Instance.ActivateJourneyInfo();
    }
    public void Deactivate()
    {
        
    }

    public void Subscribe()
    {
        
    }

    void Update()
    {
        
    }
}
