using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class SkillItemSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] TMP_Text _skillName;
    [SerializeField] Transform _skillAttributePos;
    [SerializeField] Sprite[] _skillFrames = new Sprite[2];
    [SerializeField] GameObject[] _skillAttributePrefabs = new GameObject[4];
    GameObject[] _skillAttributeSprites = new GameObject[4];

    SkillData _skillData;

    public SkillData SkillData
    {
        get { return _skillData; }
    }

    public Action<SkillItemSlot> OnPostUpdate;

    public void UpdateSlot(SkillData skillData = null, bool isSummon = default)
    {
        bool isExist = skillData ? true : false;
        _skillData = skillData;
        _skillIconImage.sprite = isExist ? skillData.IconImage : null;
        _skillIconImage.color = isExist ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0);

        for (int i = 0; i < _skillAttributeSprites.Length; i++)
        {
            if (_skillAttributeSprites[i] == null)
            {
                _skillAttributeSprites[i] = Instantiate(_skillAttributePrefabs[i], _skillAttributePos);
                _skillAttributeSprites[i].transform.localPosition = Vector3.zero;
                _skillAttributeSprites[i].SetActive(false);
            }
            if (skillData != null)
            {
                if (i == (int)skillData.SkillAttribute - 1)
                {
                    _skillAttributeSprites[i].SetActive(true);
                }
                else
                {
                    _skillAttributeSprites[i].SetActive(false);
                }
            }
        }

        if (_skillName != null)
        {
            _skillName.text = isExist ? skillData.Name : string.Empty;
        }

        if (isSummon)
        {
            GetComponent<Image>().sprite = skillData.IsUltimate ? _skillFrames[1] : _skillFrames[0];
            _skillIconImage.transform.localPosition = skillData.IsUltimate ? Define.SkillImagePosOffset : Vector2.zero;
        }
    }
}
