using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private GameObject _uiGame;

    protected override void Initialize()
    {
        base.Initialize();
        _uiGame = Instantiate(ObjectManager.Instance.UIGame);
    }

    #region Activate UI
    public void ActivateUIGame()
    {
        _uiGame.SetActive(true);
    }
    #endregion

    #region Deactivate UI
    public void DeactivateUIGame()
    {
        _uiGame.SetActive(false);
    }
    #endregion
}
