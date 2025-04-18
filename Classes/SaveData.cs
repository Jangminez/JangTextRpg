using System.Text.Json;

[Serializable]
public struct PlayerSaveData // 플레이어 저장 데이터 구조체
{
    public string Name { get; set; }
    public JobType Job { get; set; }
    public int Level { get; set; }
    public int Hp { get; set; }
    public float Attack { get; set; }
    public int Defense { get; set; }
    public int ItemAttack { get; set; }
    public int ItemDefense { get; set; }
    public int Gold { get; set; }
    public int clearTime { get; set; }
}
public struct ItemSaveData // 아이템 저장 데이터 구조체
{
    // 아이템의 정보들
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int Value { get; set; }
    public string Info { get; set; }
    public int Cost { get; set; }
    public bool IsOwn { get; set; }
    public bool IsEquip { get; set; }

    public ItemSaveData(string name, ItemType type, int value, string info, int cost, bool isOwn, bool isEquip)
    {
        Name = name;
        Type = type;
        Value = value;
        Info = info;
        Cost = cost;
        IsOwn = isOwn;
        IsEquip = isEquip;
    }
}

public class SaveData
{
    private const string PLAYER_SAVEDATA = "PlayerData.json";
    private const string ITEM_SAVEDATA = "ItemData.json";

    public static void SavePlayer(Player player, Action onCompleted)
    {
        // 플레이어 데이터 PlayerSaveData 클래스로 생성
        PlayerSaveData data = player.ToSaveData();

        // 해당 데이터 json 파일로 변환 후 저장 
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(PLAYER_SAVEDATA, json);

        // 완료시 콜백
        onCompleted?.Invoke();
    }

    public static Player LoadPlayer()
    {
        // 파일이 존재하지 않다면 null
        if (!File.Exists(PLAYER_SAVEDATA))
        {
            Console.WriteLine("\n저장된 데이터가 없습니다.");
            Thread.Sleep(1000);
            return null;
        }


        // json 파일 읽어오고 역직렬화
        string json = File.ReadAllText(PLAYER_SAVEDATA);
        PlayerSaveData data = JsonSerializer.Deserialize<PlayerSaveData>(json);

        Player? player;

        // 플레이어 직업에 따라 생성
        switch (data.Job)
        {
            case JobType.Warrior:
                player = new Warrior(data.Name, (int)data.Job);
                break;

            case JobType.Mage:
                player = new Mage(data.Name, (int)data.Job);
                break;

            case JobType.Archer:
                player = new Archer(data.Name, (int)data.Job);
                break;

            default:
                player = null;
                break;
        }

        // 플레이어 스탯, 인벤토리 데이터 적용
        if (player != null)
        {
            player.LoadData(data);

            // foreach (var item in data.InventoryItems)
            //player.inven.AddItemToInventory(item);
        }

        // player 반환
        return player;
    }

    // 아이템 저장
    public static void SaveItemsToFile(Item[] items)
    {
        if (items == null) return;

        // 아이템 리스트로 저장
        var saveList = items.Select(item => item.ToSaveData()).ToList();

        // json 형식으로 변환 후 저장
        string json = JsonSerializer.Serialize(saveList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ITEM_SAVEDATA, json);
    }

    // 아이템 불러오기 또는 생성
    public static Item[] LoadItems()
    {
        Item[] items;

        // 파일이 존재한다면 불러오기
        if (File.Exists(ITEM_SAVEDATA))
        {
            string json = File.ReadAllText(ITEM_SAVEDATA);
            var saveList = JsonSerializer.Deserialize<List<ItemSaveData>>(json);
            items = saveList.Select(Item.FromSaveData).ToArray();
        }
        else
        {
            // 없다면 새로 생성
            items = ItemManager.Instance.CreateItem();
        }

        return items;
    }
}