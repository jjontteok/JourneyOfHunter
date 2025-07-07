using System;
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

    public Sprite SkillIconSprite { get { return _skillIconImage.sprite; } }

    bool _isCoolTime = false;
    float _currentTime = 0f;
    float _coolTime;
    Define.SkillAttribute _skillAttribute;
    UIEffectsManager _effectsManager;

    readonly Color _coolTimeColor = new Color(0, 0, 0, 150f / 255);
    readonly Color _intervalColor = new Color(0, 0, 150f / 255, 150f / 255);

    private void Awake()
    {
        ReleaseIconSlot();
        _effectsManager = GetComponent<UIEffectsManager>();
    }

    void Update()
    {
        if (_isCoolTime)
        {
            _currentTime -= Time.deltaTime;
            _skillCoolTimeImage.fillAmount = _currentTime / _coolTime;
            if (_currentTime <= 0f)
            {
                FinishIconCoolTime();
            }
        }
    }

    public virtual void StartIconCoolTime(float cool)
    {
        //var reduction = PlayerManager.Instance.Player.PlayerStatus.GetCoolTimeDecrease();
        //if(!Mathf.Approximately(reduction, 0f))
        //{
        //    _coolTime = _defaultCoolTime * (1 + reduction / 100);
        //}
        //else
        //{
        //    _coolTime = _defaultCoolTime;
        //}
        //_currentTime = _coolTime;
        _currentTime = cool;
        _coolTime = cool;
        _skillCoolTimeImage.color = _coolTimeColor;
        _isCoolTime = true;
    }

    public virtual void SetIconSlot(SkillData skillData)
    {
        _skillIconImage.sprite = skillData.SkillIcon;
        _skillIconImage.color = Color.white;
        _skillCoolTimeImage.color = Color.clear;
        //_defaultCoolTime = skillData.CoolTime;
        _isCoolTime = false;
        _skillAttribute = skillData.SkillAttribute;
    }

    public virtual void ReleaseIconSlot()
    {
        _skillIconImage.sprite = null;
        _skillIconImage.color = Color.clear;
        _skillCoolTimeImage.color = Color.clear;
        _skillIntervalTimeImage.color = Color.clear;
        _skillAttribute = Define.SkillAttribute.None;
    }

    public virtual void LockIconSlot()
    {
        _skillIconImage.sprite = _lockImage;
        _skillIconImage.color = Color.white;
        _skillCoolTimeImage.color = Color.clear;
    }

    public void OnOffSkillIntervalImage(bool flag)
    {
        _skillIntervalTimeImage.color = flag ? _intervalColor : Color.clear;
    }

    protected virtual void FinishIconCoolTime()
    {
        _skillCoolTimeImage.color = Color.clear;
        _isCoolTime = false;
    }

    public void UpdateAttributeEffect(Define.TimeOfDayType type)
    {
        switch (type)
        {
            case Define.TimeOfDayType.Morning:
                if (_skillAttribute == Define.SkillAttribute.Water || _skillAttribute == Define.SkillAttribute.Light)
                {
                    _effectsManager.Run(_effectsManager.Settings[0].Name);
                }
                else
                {
                    _effectsManager.Kill(_effectsManager.Settings[0].Name);
                }
                break;
            case Define.TimeOfDayType.Noon:
                if (_skillAttribute == Define.SkillAttribute.Fire || _skillAttribute == Define.SkillAttribute.Light)
                {
                    _effectsManager.Run(_effectsManager.Settings[0].Name);
                }
                else
                {
                    _effectsManager.Kill(_effectsManager.Settings[0].Name);
                }
                break;
            case Define.TimeOfDayType.Evening:
                if (_skillAttribute == Define.SkillAttribute.Fire || _skillAttribute == Define.SkillAttribute.Dark)
                {
                    _effectsManager.Run(_effectsManager.Settings[0].Name);
                }
                else
                {
                    _effectsManager.Kill(_effectsManager.Settings[0].Name);
                }
                break;
            case Define.TimeOfDayType.Night:
                if (_skillAttribute == Define.SkillAttribute.Water || _skillAttribute == Define.SkillAttribute.Dark)
                {
                    _effectsManager.Run(_effectsManager.Settings[0].Name);
                }
                else
                {
                    _effectsManager.Kill(_effectsManager.Settings[0].Name);
                }
                break;
        }
    }
}
