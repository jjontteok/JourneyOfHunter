using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/ConsumerItem")]
public class ConsumerItemData : ItemData
{
    public ConsumerItemStatus ConsumerItemStatus;
    public Define.ConsumeTarget ConsumerType;
    public float SustainmentTime;
}

[System.Serializable]
public class ConsumerItemStatus
{
    // * 아이템 사용 시 실행
    //- 사용 타겟에 따라 효과를 적용함
    public void ApplyChanges(Define.ConsumeTarget target, int value, Define.StatusType status=default)
    {
        switch(target)
        {
            
        }
    }
}