using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private GameObject _uiMain;
    private GameObject _uiGame;
    private CanvasGroup _canvasGroup;
    UI_Main ui_main;
    UI_Game ui_game;

    public UI_Main UI_Main
    {
        get { return ui_main; }
    }

    public UI_Game UI_Game
    {
        get { return ui_game; }
    }

    protected override void Initialize()
    {
        base.Initialize();
        _uiMain = Instantiate(ObjectManager.Instance.UIMain);
        _uiGame = Instantiate(ObjectManager.Instance.UIGame);
        _canvasGroup = _uiGame.GetComponent<CanvasGroup>();

        //빌드 시 로그 확인용 임시 텍스트
        ui_game = _uiGame.GetComponent<UI_Game>();
        PlayerManager.Instance.SkillSystem.SubscribeStatusEffect();
        ui_main = _uiMain.GetComponent<UI_Main>();

        ui_main.OnPlayerNameInputted += (name) => ui_game.PlayerName = name;

        ui_main.OnStartButtonClicked += DeactivateUIMain;
        ui_main.OnStartButtonClicked += ActivateUIGame;

        ActivateUIMain();
        DeactivateUIGame();
    }

    #region Activate UI
    public void ActivateUIMain()
    {
        _uiMain.SetActive(true);
    }

    public void ActivateUIGame()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
    }
    #endregion

    #region Deactivate UI
    public void DeactivateUIMain()
    {
        _uiMain.SetActive(false);
    }

    public void DeactivateUIGame()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
    }
    #endregion
}
