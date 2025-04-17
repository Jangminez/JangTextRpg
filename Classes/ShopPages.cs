public class Shop
{
    // 상점의 메인 화면
    public void ShopMainPage(Player player)
    {
        Item[] items = ItemManager.Instance.items;

        while (true)
        {
            // 화면 출력
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");

            Console.WriteLine("[아이템 목록]");

            foreach (Item item in items)
            {
                Console.Write("- ");
                item.PrintItemInfoShop(false);
            }

            // 선택 입력 유효성 검사
            int choice = Game.InputValidator("1. 아이템 구매\n2. 아이템 판매\n0. 나가기", Game.CHOICE_ACTION, 0, 2);

            switch (choice)
            {
                case 1: // 아이템 구매
                    ShopPurchasePage(player, items);
                    break;

                case 2: // 아이템 판매
                    ShopSellPage(player);
                    break;

                case 0: // 나가기
                    return;
            }
        }
    }

    // 상점의 구매 탭
    void ShopPurchasePage(Player player, Item[] items)
    {
        while (true)
        {
            // 화면 출력
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write($"- {i + 1} ");
                items[i].PrintItemInfoShop(false);
            }

            // 선택 입력 유효성 검사
            int choice = Game.InputValidator("0. 나가기", Game.CHOICE_ACTION, 0, items.Length);

            if (choice == 0) return; // 돌아가기
            else
            {
                Item item = items[choice - 1];  // 선택한 아이템

                // 아이템 구매
                ItemManager.Instance.BuyItem(player, item, (onBought) =>
                {
                    // 구매 성공 시 
                    if (onBought)
                    {
                        player.stats.Gold -= item.itemStat.Cost;
                        Thread.Sleep(1000);
                    }

                    else Thread.Sleep(1000); // 1초 뒤 다시 처음으로  
                });
            }
        }
    }

    public void ShopSellPage(Player player)
    {
        while (true)
        {
            // 화면 출력
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < player.inven.Items.Count; i++)
            {
                Console.Write($"- {i + 1} ");
                player.inven.GetItemUseIndex(i).PrintItemInfoShop(true);
            }

            // 선택 입력 유효성 검사
            int choice = Game.InputValidator("0. 나가기", Game.CHOICE_ACTION, 0, player.inven.Items.Count);

            if (choice == 0) return; // 돌아가기
            else
            {
                Item item = player.inven.GetItemUseIndex(choice - 1);
                //아이템 판매
                ItemManager.Instance.SellItem(player, item);
            }
        }
    }
}