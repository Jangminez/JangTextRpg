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
}
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

    public void BuyItem(int gold, Action<bool> onBought)
    {
        // 이미 소유중이라면 리턴
        if (IsOwn)
        {
            Console.WriteLine("\n이미 구매한 아이템입니다.");
            onBought?.Invoke(false);
            return;
        }

        // 골드가 충분하면 구매
        else if (gold >= itemStat.Cost)
        {
            IsOwn = true;
            Console.WriteLine("\n구매를 완료했습니다.");
            onBought?.Invoke(true);
        }

        // 골드가 부족하면 구매 실패
        else if (gold < itemStat.Cost)
        {
            Console.WriteLine("\nGold가 부족합니다.");
            onBought?.Invoke(false);
        }
    }

    // 아이템 장착 및 해제
    public void SetEquipment(Player player)
    {
        // 장착 여부 변경
        IsEquip = !IsEquip;

        // 장착 여부에 따른 스탯 +, -
        switch (Type)
        {
            case ItemType.Weapon:
                player.stats.ItemAttack += IsEquip ? itemStat.Value : -itemStat.Value;
                break;

            case ItemType.Armor:
                player.stats.ItemDefense += IsEquip ? itemStat.Value : -itemStat.Value;
                break;
        }

    }

    // 아이템 정보 출력 (상점)
    public void PrintItemInfoShop()
    {
        Console.WriteLine($"{itemStat.Name}    | {GetItemType(Type)} +{itemStat.Value}    | {itemStat.Info}   | {(IsOwn ? "구매완료" : itemStat.Cost + " G")}");
    }

    // 아이템 정보 출력 (인벤토리)
    public void PrintItemInfoInventory()
    {
        Console.WriteLine($"{(IsEquip ? "[E]" : "")}{itemStat.Name}    | {GetItemType(Type)} +{itemStat.Value}    | {itemStat.Info}");
    }

    // 아이템 타입 string 변경
    private string GetItemType(ItemType type)
    {
        return type switch
        {
            ItemType.Weapon => "공격력",
            ItemType.Armor => "방어력",
            _ => "???"
        };
    }
}