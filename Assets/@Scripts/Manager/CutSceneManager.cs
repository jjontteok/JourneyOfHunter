using UnityEngine;

// 이거 없애보자
public class CutSceneManager : Singleton<CutSceneManager>,IEventSubscriber ,IDeactivateObject
{
    private GameObject _cutScene;

    protected override void Initialize()
    {
        base.Initialize();
        _cutScene = Instantiate(ObjectManager.Instance.GoblinKingCutScene);
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnSpawnNamedMonster += PlayCutScene;
    }
    #endregion

    #region IDeactivate
    public void Deactivate()
    {
       // _cutScene.SetActive(false);
    }
    #endregion

    public void PlayCutScene()
    {
        _cutScene?.SetActive(true);
        //_cutScene.GetComponentInChildren<CutSceneController>().PlayCutScene();
    }

}