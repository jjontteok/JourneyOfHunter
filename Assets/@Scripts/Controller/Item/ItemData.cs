using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class ItemData : Data
{
    [Tooltip("아이템 인덱스")]
    public int Id;
    public Define.ItemType Type;
    public Define.ItemValue Value;
}
