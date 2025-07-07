using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private GameObject _uiGame;
    private CanvasGroup _canvasGroup;
    UI_Game ui_game;

    protected override void Initialize()
    {
        base.Initialize();
        _uiGame = Instantiate(ObjectManager.Instance.UIGame);
        _canvasGroup = _uiGame.GetComponent<CanvasGroup>();

        //빌드 시 로그 확인용 임시 텍스트
        ui_game = _uiGame.GetComponent<UI_Game>();
    }

    #region Activate UI
    public void ActivateUIGame()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
    }
    #endregion

    #region Deactivate UI
    public void DeactivateUIGame()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
    }
    #endregion
}
