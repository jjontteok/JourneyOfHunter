using UnityEngine;

public class FieldController : MonoBehaviour, IEventSubscriber, IDeactivateObject
{
    private GameObject _treasure;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _treasure = Instantiate(ObjectManager.Instance.TreasureBoxResource);
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        PlayerManager.Instance.Player.OnPlayerCrossed += EventOccur;
    }
    #endregion


    #region IDeactivateObject
    public void Deactivate()
    {
        _treasure.SetActive(false);
    }
    #endregion


    void EventOccur()
    {
        //if (Util.Probability(0.3f))
        //{
        //    //보물상자 
        //    _treasure.SetActive(true);
        //}
        //else if (Util.Probability(0.6f))
        //{
        //    //떠상

        //}
        //else
        //{
        //    //다른 유물
        //}
    }
}
