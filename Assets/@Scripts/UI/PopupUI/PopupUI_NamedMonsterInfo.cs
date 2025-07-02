using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_NamedMonsterInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _monsterNameText;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] Image _hpBar;
    [SerializeField] Image _timeBar;

    private int _minute;
    private int _second;

    private void OnEnable()
    {
        TimeManager.Instance.OnNamedMonsterTimeChanged += UpdateNamedMonsterTime;
        NamedMonsterController.s_OnNamedMonsterGetDamage += UpdateNamedMonsterHpBar;
        NamedMonsterController.s_OnNamedMonsterSet += SetNamedMonsterHpBar;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnNamedMonsterTimeChanged -= UpdateNamedMonsterTime;
        NamedMonsterController.s_OnNamedMonsterGetDamage -= UpdateNamedMonsterHpBar;
        NamedMonsterController.s_OnNamedMonsterSet -= SetNamedMonsterHpBar;
    }

    void SetNamedMonsterHpBar(float currentHp, float maxHp)
    {
        _hpText.text = $"{currentHp} / {maxHp}";
        _hpBar.fillAmount = currentHp / maxHp;
    }

    void UpdateNamedMonsterHpBar(float currentHp, float maxHp)
    {
        _hpText.text = $"{currentHp} / {maxHp}";
        _hpBar.fillAmount = currentHp / maxHp;
    }

    void UpdateNamedMonsterTime(float time)
    {
        _minute = (int)time / 60;
        _second = (int)time % 60;
        _timeText.text = _minute.ToString("00") + " : " + _second.ToString("00");
        _timeBar.fillAmount = time / 100;
    }
}
