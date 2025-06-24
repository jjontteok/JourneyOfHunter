using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private GameObject _uiGame;
    UI_Game ui_game;

    public TMP_Text TemporaryText
    {
        get { return ui_game.temporaryText; }
        set { ui_game.temporaryText = value; }
    }

    public TMP_Text TemporaryTimeText
    {
        get { return ui_game.temporaryTimeText; }
        set { ui_game.temporaryTimeText = value; }
    }

    public TMP_Text TemoraryColorChangeText
    {
        get { return ui_game.temporaryColorChangeText; }
        set { ui_game.temporaryColorChangeText = value; }
    }
    protected override void Initialize()
    {
        base.Initialize();
        _uiGame = Instantiate(ObjectManager.Instance.UIGame);

        //빌드 시 로그 확인용 임시 텍스트
        ui_game = _uiGame.GetComponent<UI_Game>();
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
