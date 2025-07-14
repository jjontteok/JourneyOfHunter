using TMPro;
using UnityEngine;

public class PopupUI_BuffText : MonoBehaviour
{
    private Coroutine _buffTextCoroutine;
    TMP_Text _buffText;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _buffText = GetComponent<TMP_Text>(); 
    }

    private void OnEnable()
    {
        if (_buffTextCoroutine != null)
        {
            StopCoroutine(_buffTextCoroutine);
        }
        _buffTextCoroutine = StartCoroutine(GetComponent<UIEffectsManager>().PerformEffect(0));
        _buffText.text = $"버프 효과 {FieldManager.Instance.FailedCount * 10}%";
    }

    private void OnDisable()
    {
        // 컴포넌트 비활성화 시에도 확실히 중지
        if (_buffTextCoroutine != null)
        {
            StopCoroutine(_buffTextCoroutine);
            GetComponent<UIEffectsManager>().InitShineColor();
            _buffTextCoroutine = null;
        }
    }
}
