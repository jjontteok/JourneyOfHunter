using UnityEngine;

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    protected override void Initialize()
    {
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
}
