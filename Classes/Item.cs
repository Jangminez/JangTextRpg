public enum ItemType { Weapon = 1, Armor };

public struct ItemStats
{
    public string Name { get; } // 아이템 이름
    public int Value { get; } // 아이템 능력치 상승값
    public string Info { get; } // 아이템 설명
    public int Cost { get; } // 아이템 가격

    // 구조체 생성자
    public ItemStats(string name, int value, string info, int cost)
    {
        Name = name;
        Value = value;
        Info = info;
        Cost = cost;
    }

    // 아이템 타입 string 변경
    public string GetItemType(ItemType type)
    {
        return type switch
        {
            ItemType.Weapon => "공격력",
            ItemType.Armor => "방어력",
            _ => "???"
        };
    }
}

[Serializable]
public class Item
{

    public ItemType Type { get; }
    public ItemStats itemStat;
    public bool IsOwn { get; private set; }
    public bool IsEquip { get; private set; }

    // 아이템 생성자
    public Item(string name, ItemType type, int value, string info, int cost)
    {
        Type = type;
        itemStat = new ItemStats(name, value, info, cost);
        IsOwn = false;
        IsEquip = false;
    }

    // Item SaveData로 저장
    public ItemSaveData ToSaveData()
    {
        // 각 아이템 정보들 ItemSaveData에 저장하여 반환
        return new ItemSaveData(
            itemStat.Name,
            Type,
            itemStat.Value,
            itemStat.Info,
            itemStat.Cost,
            IsOwn,
            IsEquip
        );
    }

    // Item SaveData 적용
    public static Item FromSaveData(ItemSaveData data)
    {
        // 데이터를 통해 새로운 아이템 생성
        Item item = new Item(data.Name, data.Type, data.Value, data.Info, data.Cost);

        // 소유여부 업데이트
        if (data.IsOwn)
            item.ChangeIsOwn();

        // 장착여부 업데이트
        if (data.IsEquip)
            item.ChangeIsEquip();

        return item;
    }

    public void ChangeIsOwn()
    {
        IsOwn = !IsOwn;
    }

    public void ChangeIsEquip()
    {
        IsEquip = !IsEquip;
    }

    // 아이템 정보 출력 (상점)
    public void PrintItemInfoShop()
    {
        Console.WriteLine($"{itemStat.Name,-10} | {itemStat.GetItemType(Type),-3} +{itemStat.Value,-3} | {itemStat.Info,-35} | {(IsOwn ? "구매완료" : itemStat.Cost + " G"),-10}");
    }

    // 아이템 정보 출력 (상점 - 아이템 판매)
    public void PrintItemInfoSellItem()
    {
        Console.WriteLine($"{itemStat.Name,-10} | {itemStat.GetItemType(Type),-3} +{itemStat.Value,-3} | {itemStat.Info,-35} | {itemStat.Cost * 0.85 + " G",-10}");
    }

    // 아이템 정보 출력 (인벤토리)
    public void PrintItemInfoInventory()
    {
        Console.WriteLine($"{(IsEquip ? "[E]" : "")} {itemStat.Name,-10} | {itemStat.GetItemType(Type),-3} +{itemStat.Value,-3} | {itemStat.Info}");
    }
}