public class Inventory
{
    public List<Item> invenItem = new List<Item>();

    public void InventoryPage(Player player, Item[] items)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            for (int i = 0; i < items.Length; i++)
            {
                // 소유중인 아이템만 출력
                if (items[i].IsOwn)
                {
                    Console.Write("- ");
                    items[i].PrintItemInfoInventory();

                    // 내 아이템에 존재하지 않는다면 인덱스와 함께 추가
                    if (!invenItem.Contains(items[i]))
                    {
                        invenItem.Add(items[i]);
                    }
                }
            }

            int choice = Game.InputValidator("\n1. 장착 관리\n0. 나가기", Game.CHOICE_ACTION, 0, 1);

            switch (choice)
            {
                case 0: // 나가기
                    return;

                case 1:
                    ItemEquipPage(player); // 아이템 장착 관리로 이동
                    break;
            }
        }
    }

    private void ItemEquipPage(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            for (int i = 0; i < invenItem.Count; i++)
            {
                Console.Write($"{i + 1} ");
                invenItem[i].PrintItemInfoInventory();
            }

            int choice = Game.InputValidator("0. 나가기", Game.CHOICE_ACTION, 0, invenItem.Count);

            if (choice == 0) return; // 나가기
            else // 소유중인 아이템 선택 시
            {
                // 소유중인 아이템 리스트에서 인덱스로 접근 후 장착
                Item item = invenItem[choice - 1];

                item.SetEquipment(player);
            }
        }
    }
}