using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/ConsumerItem")]
public class ConsumerItemData : ItemData
{
    public ConsumerItemStatus ConsumerItemStatus;
}

[System.Serializable]
public class ConsumerItemStatus
{
    public Define.ConsumeTarget ConsumeTarget;

    private object _consumeTargetID;

    // 아이템 사용 메서드
    public void Consume()
    {
        ApplyChanges();
    }

    // * 아이템 사용 시 실행
    //- 사용 타겟에 따라 효과를 적용함
    private void ApplyChanges(/*ref ApplyTarget applyTarget*/)
    {
        switch( ConsumeTarget)
        {
            case Define.ConsumeTarget.Player:
                _consumeTargetID = PlayerManager.Instance.Player;
                break;
            case Define.ConsumeTarget.Field:
                break;
            case Define.ConsumeTarget.Dungeon:
                break;
            case Define.ConsumeTarget.Goods:
                break;
        }
    }
}