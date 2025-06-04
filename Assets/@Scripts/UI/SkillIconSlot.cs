using UnityEngine;
using UnityEngine.UI;

public class SkillIconSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] Image _skillCoolTimeImage;
    bool _isCoolTime = false;
    float _currentTime = 0f;
    float _coolTime;

    void Start()
    {
        
    }

    void Update()
    {
        if(_isCoolTime)
        {
            _skillCoolTimeImage.fillAmount = _currentTime / _coolTime;
        }
    }
}
