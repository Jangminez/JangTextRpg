public class ItemManager
{
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get 
        {
            if(instance == null)
                instance = new ItemManager();
            return instance;
        }
    }
    public Item[] items;
    public Item[] CreateItem()
    {
        items = new Item[] {
            new Item("수련자 갑옷", ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
            new Item("무쇠 갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000),
            new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
            new Item("낡은 검", ItemType.Weapon, 2, "쉽게 볼 수 있는 낡은 검입니다.", 600),
            new Item("청동 도끼", ItemType.Weapon, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500),
            new Item("스파르타의 창", ItemType.Weapon, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3500),
            new Item("최강의 검", ItemType.Weapon, 99, "그냥 얘가 최고다.", 9999),
            new Item("최강의 갑옷", ItemType.Armor, 99, "하지만 얘도 최고다", 99999)
        };

        return items;
    }

    // 아이템 구매
    public void BuyItem(Player player, Item item, Action<bool> onBought)
    {
        // 이미 소유중이라면 리턴
        if (item.IsOwn)
        {
            Console.WriteLine("\n이미 구매한 아이템입니다.");
            onBought?.Invoke(false);
            return;
        }

        // 골드가 충분하면 구매
        else if (player.stats.Gold >= item.itemStat.Cost)
        {
            Console.WriteLine("\n구매를 완료했습니다.");

            // 아이템 IsOwn 변경 및 인벤토리 추가
            item.ChangeIsOwn();                 
            player.inven.AddItemToInventory(item);

            onBought?.Invoke(true);
        }

        // 골드가 부족하면 구매 실패
        else if (player.stats.Gold < item.itemStat.Cost)
        {
            Console.WriteLine("\nGold가 부족합니다.");
            onBought?.Invoke(false);
        }
    }

    public void SellItem(Player player, Item item)
    {
        if(item.IsEquip)
            SetEquipment(player, item);

        Console.WriteLine($"\n아이템을 판매하여 {item.itemStat.Cost * 0.85:F0} G를 얻었습니다.");

        // 아이템 인벤에서 제거 및 돈 지급
        item.ChangeIsOwn();
        player.inven.RemoveItemInventory(item);
        player.stats.Gold += (int)(item.itemStat.Cost * 0.85);
        
        Thread.Sleep(1000);
    }

    // 아이템 장착 및 해제
    public void SetEquipment(Player player, Item item)
    {
        // 장착 여부 변경
        item.ChangeIsEquip();

        // 장착 여부에 따른 스탯 +, -
        switch (item.Type)
        {
            case ItemType.Weapon:
                player.stats.ItemAttack += item.IsEquip ? item.itemStat.Value : -item.itemStat.Value;
                break;

            case ItemType.Armor:
                player.stats.ItemDefense += item.IsEquip ? item.itemStat.Value : -item.itemStat.Value;
                break;
        }
    }

}