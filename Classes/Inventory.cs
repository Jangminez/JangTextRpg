public class Inventory
{
    // 외부에선 읽기 전용으로 가져가도록 보호
    private List<Item> invenItem = new List<Item>();
    public IReadOnlyCollection<Item> Items => invenItem;

    // 인벤토리에 아이템 추가
    public void AddItemToInventory(Item item)
    {
        // 아이템이 없다면 추가
        if (!invenItem.Contains(item))
            invenItem.Add(item);
    }

    // 인벤토리에 아이템 제거
    public void RemoveItemInventory(Item item)
    {
        // 아이템이 존재한다면
        if(invenItem.Contains(item))
            invenItem.Remove(item);
    }

    public Item GetItemUseIndex(int idx)
    {
        return invenItem[idx];
    }
}