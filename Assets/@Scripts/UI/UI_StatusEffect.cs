using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusEffect : MonoBehaviour
{
    List<Image> _statusEffectImages = new List<Image>();
    [SerializeField] GameObject _statusEffectPrefab;

    const float _start = 25f;
    const float _interval = 50f;

    public void UpdateStatusEffect(Sprite sprite, bool flag)
    {
        // 버프 추가
        if (flag)
        {
            
            Image additionImage = Instantiate(_statusEffectPrefab, gameObject.transform).GetComponent<Image>();
            additionImage.sprite = sprite;
            _statusEffectImages.Add(additionImage);
        }
        // 버프 해제
        else
        {
            int deleteIdx = _statusEffectImages.FindIndex(image => image.sprite == sprite);
            if (deleteIdx < 0)
            {
                Debug.Log("해당 버프가 없습니다");
                return;
            }
            Destroy(_statusEffectImages[deleteIdx].gameObject);
            _statusEffectImages.RemoveAt(deleteIdx);
        }
        SortImageList();
    }

    void SortImageList()
    {
        for (int i = 0; i < _statusEffectImages.Count; i++)
        {
            //_statusEffectImages[i].gameObject.transform.localPosition = new Vector3(_start + _interval * i, 0, 0);
            _statusEffectImages[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(_start + _interval * i, 0, 0);
        }
    }
}
