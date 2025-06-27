using UnityEngine;

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{

    protected override void Initialize()
    {
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
