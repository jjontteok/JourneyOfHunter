using extension;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerVital : MonoBehaviour
{
    [SerializeField] Image _hpBar;
    [SerializeField] Image _mpBar;

    RectTransform _rectTransform;
    Transform _target;
    Vector3 _playerVitalOffset = new Vector3(0, 2, 0);

    public void Initialize(Transform target)
    {
        _target = target;
        _rectTransform = GetComponent<RectTransform>();
        InitPlayerVital();
    }

    void InitPlayerVital()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position + _playerVitalOffset);
        _rectTransform.position = screenPos;
    }

    private void OnEnable()
    {
        PlayerManager.Instance.Player.OnHPValueChanged += UpdatePlayerHp;
        PlayerManager.Instance.Player.OnMPValueChanged += UpdatePlayerMp;
    }

    //private void OnDisable()
    //{
    //    PlayerManager.Instance.Player.OnHPValueChanged -= UpdatePlayerHp;
    //    PlayerManager.Instance.Player.OnMPValueChanged -= UpdatePlayerMp;
    //}

    private void LateUpdate()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position + _playerVitalOffset);
        _rectTransform.position = Vector3.Lerp(transform.position, screenPos, Time.fixedDeltaTime);
    }

    void UpdatePlayerHp(float currentHP, float maxHP)
    {
        _hpBar.fillAmount = currentHP / maxHP;
    }

    void UpdatePlayerMp(float currentMP, float maxMP)
    {
        _mpBar.fillAmount = currentMP / maxMP;
    }
}
