using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

enum CatalogueType
{
    Shop,
    SellItem,
    Inventory
}

public class Pages
{
    #region  목록 출력 메서드

    /// <summary>
    /// 목록을 출력하는 메서드
    /// </summary>
    /// <param name="items"></param>
    /// <param name="type">0 = Shop, 1 = SellItem, 2 = Inventory</param>
    private void PrintCatalogue(IReadOnlyCollection<Item> items, CatalogueType type)
    {
        foreach (Item item in items)
        {
            Console.Write("- ");
            switch (type)
            {
                case CatalogueType.Shop: // 상점
                    item.PrintItemInfoShop();
                    break;

                case CatalogueType.SellItem: // 상점 - 아이템판매
                    item.PrintItemInfoSellItem();
                    break;

                case CatalogueType.Inventory: // 인벤토리
                    item.PrintItemInfoInventory();
                    break;
            }
        }
    }

    private void PrintCatalogueWithNumber(IReadOnlyCollection<Item> items, CatalogueType type)
    {
        int idx = 1;
        foreach (Item item in items)
        {
            Console.Write($"{idx} ");
            switch (type)
            {
                case CatalogueType.Shop: // 상점
                    item.PrintItemInfoShop();
                    break;

                case CatalogueType.SellItem: // 상점 - 아이템판매
                    item.PrintItemInfoSellItem();
                    break;

                case CatalogueType.Inventory: // 인벤토리
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
            BannerManager.Show("INVENTORY");
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            // 아이템 목록 출력
            PrintCatalogue(player.inven.Items, CatalogueType.Inventory);

            int choice = InputHandler.InputValidator("1. 장착 관리\n0. 나가기", 0, 1);

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
            BannerManager.Show("INVENTORY");
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            // 아이템 목록 출력
            PrintCatalogueWithNumber(player.inven.Items, CatalogueType.Inventory);

            int choice = InputHandler.InputValidator("0. 나가기", 0, player.inven.Items.Count);

            if (choice == 0) return; // 나가기
            else // 소유중인 아이템 선택 시
            {
                // 소유중인 아이템 리스트에서 인덱스로 접근 후 장착
                Item item = player.inven.GetItemUseIndex(choice - 1);

                ItemManager.Instance.SetItemEquip(player, item);
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
            BannerManager.Show("SHOP");
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");


            Console.WriteLine("[아이템 목록]");

            // 아이템 목록 출력
            PrintCatalogue(items, CatalogueType.Shop);

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
            BannerManager.Show("SHOP");
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");

            Console.WriteLine("[아이템 목록]");

            // 아이템 목록 출력
            PrintCatalogueWithNumber(items, CatalogueType.Shop);

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
            BannerManager.Show("SHOP");
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 구매할 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유골드]\n{player.stats.Gold} G\n");

            Console.WriteLine("[아이템 목록]");

            // 아이템 목록 출력
            PrintCatalogueWithNumber(player.inven.Items, CatalogueType.SellItem);

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
        // 휴식 가격
        int cost = 500;

        while (true)
        {
            Console.Clear();
            BannerManager.Show("REST");
            Console.WriteLine("휴식하기");
            Console.WriteLine($"{cost} G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.stats.Gold} G)");

            int choice = InputHandler.InputValidator("1. 휴식하기\n0. 나가기", 0, 1);

            switch (choice)
            {
                case 1:
                    player.Rest(cost);
                    break;

                case 0:
                    return;

            }
        }
    }
    #endregion

    #region 던전 관련 페이지
    // 던전 페이지
    public void DungeonPage(Player player)
    {

        while (true)
        {
            Console.Clear();
            BannerManager.Show("DUNGEON");
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            int choice = InputHandler
            .InputValidator($"1. 쉬운 던전  | 방어력 {(int)DungeonType.EASY} 이상 권장"
                            + $"\n2. 일반 던전  | 방어력 {(int)DungeonType.NORMAL} 이상 권장"
                            + $"\n3. 어려운 던전  | 방어력 {(int)DungeonType.HARD} 이상 권장"
                            + $"\n0. 나가기", 0, 3);

            // 선택에 따른 난이도의 던전 입장
            switch (choice)
            {
                case 0: return;

                case 1:
                    DungeonResultPage(player, DungeonType.EASY);
                    break;

                case 2:
                    DungeonResultPage(player, DungeonType.NORMAL);
                    break;

                case 3:
                    DungeonResultPage(player, DungeonType.HARD);
                    break;
            }
        }
    }

    // 던전 결과 페이지
    private void DungeonResultPage(Player player, DungeonType type)
    {
        if (player.stats.Hp == 0)
        {
            Console.WriteLine("\n지금은 숟가락들 힘도 없습니다...");
            Thread.Sleep(1000);
            return;
        }

        // 플레이어 던전 입장
        player.EnterDungeon();

        // 사용할 클래스 객체 생성
        Random rand = new Random();
        Dungeon dungeon = new Dungeon();

        // 던전 탐험 전 Hp, Gold 저장
        int pre_Hp = player.stats.Hp;
        int pre_Gold = player.stats.Gold;

        // 던전 탐험 시작
        dungeon.EnterDungeon(player, type, onCleared =>
        {
            // 클리어 성공
            if (onCleared)
            {
                Console.Clear();
                BannerManager.Show("DUNGEON");
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!!");
                Console.WriteLine($"{dungeon.GetTypeString(type)}을 클리어 하였습니다.\n");

                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {pre_Hp} -> {player.stats.Hp}");
                Console.WriteLine($"Gold {pre_Gold} G -> {player.stats.Gold} G");

                player.SetClearTime();  // 클리어 타임 추가

                int choice = InputHandler.InputValidator("0. 나가기", 0, 0);
                if (choice == 0) return;
            }

            // 클리어 실패
            else
            {
                Console.Clear();
                BannerManager.Show("DUNGEON");
                Console.WriteLine("던전 실패");
                Console.WriteLine("이런!!!!");
                Console.WriteLine($"{dungeon.GetTypeString(type)}을 클리어에 실패하였습니다.\n");

                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {pre_Hp} -> {player.stats.Hp}");

                int choice = InputHandler.InputValidator("0. 나가기", 0, 0);
                if (choice == 0) return;
            }
        });

    }
    #endregion
}