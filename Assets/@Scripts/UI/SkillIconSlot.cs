using UnityEngine;
using UnityEngine.UI;

public class SkillIconSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] Image _skillCoolTimeImage;
    [SerializeField] Sprite _lockImage;

    bool _isCoolTime = false;
    float _currentTime = 0f;
    float _coolTime;

    private void Awake()
    {
        ReleaseIconSlot();
    }

    void Update()
    {
        if(_isCoolTime)
        {
            _currentTime -= Time.deltaTime;
            _skillCoolTimeImage.fillAmount = _currentTime / _coolTime;
            if (_currentTime <= 0f)
            {
                _skillCoolTimeImage.color = new Color(0, 0, 0, 0);
                _isCoolTime = false;
            }
        }
    }

    public void StartIconCoolTime()
    {
        _skillCoolTimeImage.color = new Color(0, 0, 0, 150f / 255);
        _currentTime = _coolTime;
        _isCoolTime = true;
    }

    public void SetIconSlot(SkillData skillData)
    {
        _skillIconImage.sprite = skillData.skillIcon;
        _skillIconImage.color = Color.white;
        _skillCoolTimeImage.color = Color.clear;
        _coolTime = skillData.coolTime;
        _isCoolTime = false;
    }

    public void ReleaseIconSlot()
    {
        _skillIconImage.sprite = null;
        _skillIconImage.color = Color.clear;
        _skillCoolTimeImage.color = Color.clear;
    }

    public void LockIconSlot()
    {
        _skillIconImage.sprite = _lockImage;
        _skillIconImage.color = Color.white;
        _skillCoolTimeImage.color = Color.clear;
    }
}
