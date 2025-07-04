using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class ItemData : ScriptableObject
{
    [Tooltip("아이템 인덱스")]
    public int Id;
    public string Name;
    public string Description;

    public Sprite IconImage;
    public Define.ItemType Type;
    public Define.ItemValue Value;

    [SerializeField]
    private int count = 0;

    public int Count
    {
        get { return count; }
        set 
        {
            if (value <= 99 && value > 0)
                count = value;
            else
                Debug.Log("아이템 개수가 범위를 벗어났습니다. 적용 x");
        }
    }
}
