using TMPro;
using UnityEngine;

public class PopupUI_NamedMonsterInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _monsterNameText;
    [SerializeField] TMP_Text _timeText;

    private int _minute;
    private int _second;

    private void OnEnable()
    {
        TimeManager.Instance.OnNamedMonsterTimeChanged += UpdateNamedMonsterTime;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnNamedMonsterTimeChanged -= UpdateNamedMonsterTime;
    }

    void UpdateNamedMonsterTime(float time)
    {
        _minute = (int)time / 60;
        _second = (int)time % 60;
        _timeText.text = _minute.ToString("00") + " : " + _second.ToString("00");
    }
}
