public enum DungeonType
{
    EASY = 5,
    NORMAL = 11,
    HARD = 17
};

public class Dungeon
{
    // 각 난이도의 보상 설정
    private Dictionary<DungeonType, int> rewardDic = new()
    {
        { DungeonType.EASY, 1000 },
        { DungeonType.NORMAL, 1700 },
        { DungeonType.HARD, 2500 },
    };

    // 랜덤한 숫자 생성을 위한 랜덤 객체 생성
    Random rand = new Random();

    public void EnterDungeon(Player player, DungeonType type, Action<bool> onCleared)
    {
        bool isClear = false;

        // 권장 방어력 보다 높을 때 
        if (player.stats.Defense >= (int)type)
            isClear = DungeonClear(player, type, true);

        // 권장 방어력 보다 낮을 때
        else
        {
            // 40% 확률로 실패
            if (rand.Next(1, 10) <= 4)
                isClear = DungeonClear(player, type, false);

            // 60% 확률로 성공
            else
                isClear = DungeonClear(player, type, true);
        }

        // 클리어 콜백 호출
        onCleared?.Invoke(isClear);
    }

    private bool DungeonClear(Player player, DungeonType type, bool isClear)
    {
        // 추가 체력 감소에 쓰일 방어력 차이
        int defenseDiff = player.stats.Defense - (int)type;
        // 추가 보상 % 계산
        float bonusReward = 1f + (rand.Next((int)player.stats.Attack, (int)(player.stats.Attack * 2)) / 100f);

        if (isClear)
        {
            // 체력 감소량
            int hpDecrease = rand.Next(20 + defenseDiff, 35 + defenseDiff);
            // 보상 골드 계산
            int rewardGold = (int)(rewardDic[type] * bonusReward);

            // 클리어 보상 적용
            player.ClearDungeon(hpDecrease, rewardGold);
        }

        else
        {
            // 체력 절반 감소, 보상 X
            player.ClearDungeon(player.stats.Hp / 2, 0);
        }

        return isClear;
    }

    public string GetTypeString(DungeonType type)
    {
        return type switch
        {
            DungeonType.EASY => "쉬운 던전",
            DungeonType.NORMAL => "일반 던전",
            DungeonType.HARD => "어려운 던전",
            _ => "오류 던전"
        };
    }
}