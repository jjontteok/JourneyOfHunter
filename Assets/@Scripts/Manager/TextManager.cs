using TMPro;
using UnityEngine;

public class TextManager : Singleton<TextManager>, IEventSubscriber, IDeactivateObject
{
    //private Dictionary<string, GameObject> _damageTextList;

    [SerializeField] private Canvas _canvas;
    private GameObject _damageTextPool;
    private GameObject _treasureTextPool;

    PlayerController _player;
    protected override void Initialize()
    {
        _canvas = new GameObject("UI_Text").AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;

        _damageTextPool = new GameObject("DamageTextPool");
        _treasureTextPool = new GameObject("TreasureTextPool");

        _damageTextPool.transform.SetParent(_canvas.transform, true);
        _treasureTextPool.transform.SetParent(_canvas.transform, true);

        _player = PlayerManager.Instance.Player;
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DamageTextEvent.OnDamageTaken += ActivateDamageText;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {

    }
    #endregion

    void ActivateDamageText(Vector3 pos, float damage, bool isCritical)
    {
        GameObject damageText = PoolManager.Instance.GetObjectFromPool<DamageTextController>
            (pos, isCritical ? "CriticalDamageText" : "NormalDamageText");
        damageText.GetComponent<DamageTextController>().OriginPos = pos;
        TMP_Text textMeshPro = damageText.GetComponent<TMP_Text>();
        textMeshPro.text = damage.ToString();
        damageText.transform.SetParent(_damageTextPool.transform, true);
        damageText.SetActive(true);
    }

    public void ActivateRewardText(Vector3 pos, string treasure, int amount)
    {

        //활성화 되어 있는 텍스트 찾기 -> 이미 활성화된 텍스트 위로 올려줌
        RewardTextController[] activeTexts = _treasureTextPool.GetComponentsInChildren<RewardTextController>(false);
        foreach(RewardTextController activeText in activeTexts)
        {
            activeText.UpdateTextPos();
        }
       
        //현재 텍스트
        GameObject treasureText = PoolManager.Instance.
            GetObjectFromPool<RewardTextController>(pos, "TreasureText");
        treasureText.GetComponent<RewardTextController>().OriginPos = pos;
        treasureText.transform.SetParent(_treasureTextPool.transform, true);
        TMP_Text textMeshPro = treasureText.GetComponent<TMP_Text>();
        textMeshPro.text = $"{treasure} 획득 +{amount}";
    }
}
