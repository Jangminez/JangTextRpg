public class Pages
{
    #region  목록 출력 메서드

    /// <summary>
    /// 목록을 출력하는 메서드
    /// </summary>
    /// <param name="items"></param>
    /// <param name="type">0 = Shop, 1 = SellItem, 2 = Inventory</param>
    private void PrintCatalogue(IReadOnlyCollection<Item> items, int type)
    {
        foreach (Item item in items)
        {
            Console.Write("- ");
            switch (type)
            {
                case 0: // 상점
                    item.PrintItemInfoShop();
                    break;

                case 1: // 상점 - 아이템판매
                    item.PrintItemInfoSellItem();
                    break;

                case 2: // 인벤토리
                    item.PrintItemInfoInventory();
                    break;
            }
        }
    }

    private void PrintCatalogueWithNumber(IReadOnlyCollection<Item> items, int type)
    {
        int idx = 1;
        foreach (Item item in items)
        {
            Console.Write($"{idx} ");
            switch (type)
            {
                case 0: // 상점
                    item.PrintItemInfoShop();
                    break;

                case 1: // 상점 - 아이템판매
                    item.PrintItemInfoSellItem();
                    break;

                case 2: // 인벤토리
                    item.PrintItemInfoInventory();
                    break;
            }

            idx++;
        }
    }
    #endregion

    #region 인벤토리 관련 페이지
    // 인벤토리 페이지
    public void InventoryPage(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            // 아이템 목록 출력
            PrintCatalogue(player.inven.Items, 2);

            int choice = InputHandler.InputValidator("\n1. 장착 관리\n0. 나가기", 0, 1);

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

            // 아이템 목록 출력
            PrintCatalogueWithNumber(player.inven.Items, 2);

            int choice = InputHandler.InputValidator("0. 나가기", 0, player.inven.Items.Count);

            if (choice == 0) return; // 나가기
            else // 소유중인 아이템 선택 시
            {
                // 소유중인 아이템 리스트에서 인덱스로 접근 후 장착
                Item item = player.inven.GetItemUseIndex(choice - 1);

                ItemManager.Instance.SetEquipment(player, item);
            }
        }
    }
    #endregion

    #region  상점 관련 페이지 
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

            // 아이템 목록 출력
            Console.WriteLine("[아이템 목록]");

            PrintCatalogue(items, 0);

            // 선택 입력 유효성 검사
            int choice = InputHandler.InputValidator("1. 아이템 구매\n2. 아이템 판매\n0. 나가기", 0, 2);

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
            
            // 아이템 목록 출력
            PrintCatalogueWithNumber(items, 0);

            // 선택 입력 유효성 검사
            int choice = InputHandler.InputValidator("0. 나가기", 0, items.Length);

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
            
            // 아이템 목록 출력
            PrintCatalogueWithNumber(player.inven.Items, 1); 

            // 선택 입력 유효성 검사
            int choice = InputHandler.InputValidator("0. 나가기", 0, player.inven.Items.Count);

            if (choice == 0) return; // 돌아가기
            else
            {
                Item item = player.inven.GetItemUseIndex(choice - 1);
                //아이템 판매
                ItemManager.Instance.SellItem(player, item);
            }
        }
    }
    #endregion

    #region 휴식 관련 페이지
    // 휴식 페이지
    public void RestPage(Player player)
    {
        Console.Clear();
        Console.WriteLine("휴식하기");
        Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.stats.Gold} G");
    }
    #endregion
}