using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;

    public Sprite IconImage;
    public Define.ItemType Type;
    public Define.ItemValue Value;
}
