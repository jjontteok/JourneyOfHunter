using UnityEngine;
using UnityEngine.UI;

public class UltimateSkillIconSlot : SkillIconSlot
{
    [SerializeField] Image _skillIconBoundImage;
    // 0번이 회색(==쿨타임), 1번이 금색(==사용 가능)
    [SerializeField] Sprite[] _skillIconBoundSprites = new Sprite[2];

    public override void StartIconCoolTime()
    {
        base.StartIconCoolTime();
        _skillIconBoundImage.sprite = _skillIconBoundSprites[0];
    }

    public override void SetIconSlot(SkillData skillData)
    {
        base.SetIconSlot(skillData);
        _skillIconBoundImage.sprite = _skillIconBoundSprites[1];
    }

    public override void ReleaseIconSlot()
    {
        base.ReleaseIconSlot();
        _skillIconBoundImage.sprite = _skillIconBoundSprites[0];
    }

    public override void LockIconSlot()
    {
        base.LockIconSlot();
        _skillIconBoundImage.sprite = _skillIconBoundSprites[0];
    }

    protected override void FinishIconCoolTime()
    {
        _skillIconBoundImage.sprite = _skillIconBoundSprites[1];
    }
}
