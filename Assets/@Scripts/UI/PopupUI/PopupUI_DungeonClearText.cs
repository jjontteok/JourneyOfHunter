using System.Collections;
using TMPro;
using UnityEngine;

//나중에 하자...오늘은 안할랭
public class PopupUI_DungeonClearText : MonoBehaviour
{
    UIEffectsManager _uiEffectManager;
    TMP_Text _dungeonClearText;


    WaitForSeconds _deactivateTime = new WaitForSeconds(2f);
    public bool IsClear
    {
        get; set;
    }

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _uiEffectManager = GetComponent<UIEffectsManager>();
        _dungeonClearText = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        _dungeonClearText.text = "던전 클리어 ";
        _dungeonClearText.text += IsClear ? "성공" : "실패";
        _dungeonClearText.color = IsClear ? Color.cyan : Color.red;
        StartCoroutine(_uiEffectManager.PerformEffect(0));
        StartCoroutine(DeactivateText());
    }

    IEnumerator DeactivateText()
    {
        yield return _deactivateTime;
        gameObject.SetActive(false);
    }

}
