using UnityEngine;

[System.Serializable]
public class Data : ScriptableObject
{
    public string Name;
    public string Description;

    public Sprite IconImage;

    [SerializeField]
    private int count = 0;

    public int Count
    {
        get { return count; }
        set
        {
            if (value <= 99 && value >= 0)
                count = value;
            else
                Debug.Log("아이템 개수가 범위를 벗어났습니다. 적용 x");
        }
    }
}
