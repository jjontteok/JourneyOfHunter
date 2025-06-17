using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class SkillIconSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] Image _skillCoolTimeImage;
    [SerializeField] Image _skillIntervalTimeImage;
    [SerializeField] Sprite _lockImage;

    // 스킬 아이콘 클릭 시 스킬 발동하는 이벤트
    public Action OnClickSkillIcon;

    bool _isCoolTime = false;
    float _currentTime = 0f;
    float _coolTime;

    readonly Color _coolTimeColor = new Color(0, 0, 0, 150f / 255);
    readonly Color _intervalColor = new Color(0, 0, 150f / 255, 150f / 255);

    private void Awake()
    {
        ReleaseIconSlot();
    }

    void Update()
    {
        if (_isCoolTime)
        {
            _currentTime -= Time.deltaTime;
            _skillCoolTimeImage.fillAmount = _currentTime / _coolTime;
            if (_currentTime <= 0f)
            {
                _skillCoolTimeImage.color = Color.clear;
                _isCoolTime = false;
            }
        }
    }

    public void StartIconCoolTime()
    {
        _skillCoolTimeImage.color = _coolTimeColor;
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
        _skillIntervalTimeImage.color = Color.clear;
    }

    public void LockIconSlot()
    {
        _skillIconImage.sprite = _lockImage;
        _skillIconImage.color = Color.white;
        _skillCoolTimeImage.color = Color.clear;
    }

    public void OnOffSkillIntervalImage(bool flag)
    {
        _skillIntervalTimeImage.color = flag ? _intervalColor : Color.clear;
    }
}
