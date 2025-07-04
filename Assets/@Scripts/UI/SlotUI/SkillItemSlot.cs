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
    [SerializeField] GameObject[] _skillAttributePrefabs = new GameObject[4];
    GameObject[] _skillAttributeSprites = new GameObject[4];

    SkillData _skillData;

    public SkillData SkillData
    {
        get { return _skillData; }
    }

    public Action<SkillItemSlot> OnPostUpdate;

    public void UpdateSlot(SkillData skillData = null)
    {
        bool isExist = skillData ? true : false;
        _skillData = skillData;
        _skillIconImage.sprite = isExist ? skillData.SkillIcon : null;
        _skillIconImage.color = isExist ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0);

        for (int i = 0; i < _skillAttributeSprites.Length; i++)
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

        _skillName.text = isExist ? skillData.SkillName : string.Empty;
    }

    private void Awake()
    {
        _skillIconImage.color = new Color(0, 0, 0, 0);
        _skillName.text = string.Empty;
        for (int i = 0; i < _skillAttributeSprites.Length; i++)
        {
            _skillAttributeSprites[i] = Instantiate(_skillAttributePrefabs[i], _skillAttributePos);
            _skillAttributeSprites[i].transform.localPosition = Vector3.zero;
            _skillAttributeSprites[i].SetActive(false);
        }
    }
}
