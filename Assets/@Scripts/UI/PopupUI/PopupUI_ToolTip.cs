using UnityEngine;
using TMPro;

public class PopupUI_ToolTip : MonoBehaviour
{
    [SerializeField] GameObject _toolTip;
    [SerializeField] TMP_Text _toolTipName;
    [SerializeField] TMP_Text _toolTipContent;

    public void SetToolTip(Vector2 pos, string name, string content, Color nameColor = default)
    {
        if(nameColor != default)
            _toolTipName.color = nameColor;
        else
            _toolTipName.color = Color.white;
        _toolTip.transform.position = pos;
        _toolTipName.text = name;
        _toolTipContent.text = content;
    }
}
