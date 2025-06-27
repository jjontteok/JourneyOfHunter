using UnityEditor;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Tooltip("아이템 인덱스")]
    public int Id;
    public string Name;
    public string Description;

    public Sprite IconImage;
    public Define.ItemType Type;
    public Define.ItemValue Value;
}
