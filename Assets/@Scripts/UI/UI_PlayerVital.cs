using extension;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerVital : MonoBehaviour
{
    [SerializeField] Image _hpBar;
    [SerializeField] Image _mpBar;

    Transform _target;
    Vector3 _playerVitalOffset = new Vector3(0, 2, 0);

    public void Initialize(Transform target)
    {
        _target = target;
    }

    private void OnEnable()
    {
        PlayerController.OnHPValueChanged += UpdatePlayerHp;
        PlayerController.OnMPValueChanged += UpdatePlayerMp;
    }

    private void OnDisable()
    {
        PlayerController.OnHPValueChanged -= UpdatePlayerHp;
        PlayerController.OnMPValueChanged -= UpdatePlayerMp;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position + _playerVitalOffset);
        transform.position = screenPos;
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
