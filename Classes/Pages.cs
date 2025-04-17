public class Pages
{
    // 인벤토리 페이지
    public void InventoryPage(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            for (int i = 0; i < player.inven.Items.Count; i++)
            {
                Console.Write("- ");
                player.inven.GetItemUseIndex(i).PrintItemInfoInventory();
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

    // 아이템 장착 페이지
    private void ItemEquipPage(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            for (int i = 0; i < player.inven.Items.Count; i++)
            {
                Console.Write($"{i + 1} ");
                player.inven.GetItemUseIndex(i).PrintItemInfoInventory();
            }

            int choice = Game.InputValidator("0. 나가기", Game.CHOICE_ACTION, 0, player.inven.Items.Count);

            if (choice == 0) return; // 나가기
            else // 소유중인 아이템 선택 시
            {
                // 소유중인 아이템 리스트에서 인덱스로 접근 후 장착
                Item item = player.inven.GetItemUseIndex(choice - 1);

                ItemManager.Instance.SetEquipment(player, item);
            }
        }
    }

    // 휴식 페이지
    public void RestPage(Player player)
    {
        Console.Clear();
        Console.WriteLine("휴식하기");
        Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.stats.Gold} G");
    }
}