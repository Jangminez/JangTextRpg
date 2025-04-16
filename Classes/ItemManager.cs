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

    public Item[] CreateItem()
    {
        Item[] items = new Item[] {
            new Item("수련자 갑옷", ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
            new Item("무쇠 갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000),
            new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
            new Item("낡은 검", ItemType.Weapon, 2, "쉽게 볼 수 있는 낡은 검입니다.", 600),
            new Item("청동 도끼", ItemType.Weapon, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500),
            new Item("스파르타의 창", ItemType.Weapon, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3500)
        };

        return items;
    }

}