using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/ConsumerItem")]
public class ConsumerItemData : ItemData
{
    public ConsumerItemStatus ConsumerItemStatus;
}

[System.Serializable]
public class ConsumerItemStatus
{
    // 이건 각 봐서해야할듯..
    public int Count = 0;
    
    public void Consume()
    {
        if(Count <= 0)
        {
            Debug.Log("아이템이 존재하지 않습니다.");
        }
        else
        {
            Count--;
        }
    }

    private void ApplyChanges(/*ref ApplyTarget applyTarget*/)
    {

    }
}