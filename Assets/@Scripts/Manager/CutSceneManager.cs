using UnityEngine;

// 이거 없애보자
public class CutSceneManager : Singleton<CutSceneManager>,IEventSubscriber ,IDeactivateObject
{
    private GameObject _cutScene;
    CutSceneController _cutSceneController;

    protected override void Initialize()
    {
        base.Initialize();
        _cutScene = Instantiate(ObjectManager.Instance.GoblinKingCutScene);
        _cutSceneController = _cutScene.GetComponentInChildren<CutSceneController>();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += PlayCutScene;
        _cutSceneController.OnCutSceneFinished += FinishCutScene;
    }
    #endregion

    #region IDeactivate
    public void Deactivate()
    {
        _cutScene.SetActive(false);
    }
    #endregion

    public void PlayCutScene()
    {
        Debug.Log("컷신 실행");
        _cutScene?.SetActive(true);
        _cutSceneController.PlayCutScene();
    }

    void FinishCutScene()
    {

    }
}