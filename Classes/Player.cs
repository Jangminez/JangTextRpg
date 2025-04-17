public enum JobType { Warrior = 1, Mage, Archer };

public struct Stats
{
    private int level;
    public int Level
    {
        get => level;
        set => level = Math.Max(0, value);
    }

    private int hp;
    public int Hp
    {
        get => hp;

        // 체력은 0 ~ 100 사이 값 고정
        set => hp = Math.Min(Math.Max(0, value), 100);
    }

    private float attack;
    public float Attack
    {
        // 기본 공격력 + 아이템 추가 공격력 반환
        get => attack + itemAttack;
        set => attack = Math.Max(0, value);
    }

    private int defense;
    public int Defense
    {
        // 기본 방어력 + 아이템 추가 방어력 반환
        get => defense + itemDefense;
        set => defense = Math.Max(0, value);
    }

    private int itemAttack;
    public int ItemAttack
    {
        get => itemAttack;
        set => itemAttack = Math.Max(0, value);
    }

    private int itemDefense;
    public int ItemDefense
    {
        get => itemDefense;
        set => itemDefense = Math.Max(0, value);
    }

    private int gold;
    public int Gold
    {
        get => gold;
        set => gold = Math.Max(0, value);
    }
}

public abstract class Player
{
    private JobType jobType { get; } // 플레이어 직업
    public string Name { get; } // 플레이어 이름
    public Stats stats; // 플레이어 능력치
    private int clearTime = 0;  // 클리어 횟수
    public Inventory inven = new Inventory(); // 인벤토리 객체 생성

    // 생성자
    public Player(string Name, int jobType)
    {
        // 이름과 직업 타입만 받아오고 나머지는 기본값
        this.Name = Name;
        this.jobType = (JobType)jobType;

        stats.Level = 1;
        stats.Attack = 10;
        stats.ItemAttack = 0;
        stats.Defense = 5;
        stats.ItemDefense = 0;
        stats.Hp = 100;
        stats.Gold = 1500;
    }

    // 상태창 페이지
    public void StatusPage()
    {
        while (true)
        {
            // 콘솔 창 클리어 및 캐릭터 정보 표시
            Console.Clear();
            BannerManager.Show("STATUS");
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시 됩니다.\n");
            Console.WriteLine($"닉네임: {Name}");
            Console.WriteLine($"Lv. {stats.Level:00}");
            Console.WriteLine($"Chad ({GetJobType(jobType)})");
            Console.WriteLine($"공격력 : {stats.Attack:F1} " + GetItemStat(stats.ItemAttack));
            Console.WriteLine($"방어력 : {stats.Defense} " + GetItemStat(stats.ItemDefense));
            Console.WriteLine($"체력 : {stats.Hp}");
            Console.WriteLine($"Gold : {stats.Gold} G");

            Console.WriteLine("\n0. 나가기");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

            // 입력 값 체크
            if (!int.TryParse(Console.ReadLine(), out int input) || input != 0)
            {
                Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                Thread.Sleep(1000);
            }
            else return;
        }
    }

    // 휴식하기 메서드
    abstract public void Rest(int cost);

    // 나중에 던전 입장 시에 사용
    abstract public void EnterDungeon();

    // 던전 클리어시
    public void ClearDungeon(int hpDecrease, int rewardGold)
    {
        // 체력 감소 및 골드 추가
        stats.Hp -= hpDecrease;
        stats.Gold += rewardGold;
    }

    // 클리어 타임으로 레벨 관리
    public void SetClearTime()
    {
        // 클리어 횟수 증가
        clearTime++;

        // 던전 클리어 횟수와 레벨이 같으면 레벨업
        if (stats.Level == clearTime)
            LevelUp();
    }

    // 레벨업
    private void LevelUp()
    {
        // 레벨 증가 및 클리어 횟수 초기화
        stats.Level++;
        clearTime = 0;

        Console.WriteLine($"\n레벨업 하셨습니다!! 현재 레벨 -> Lv. {stats.Level}\n");
        Console.WriteLine($"공격력: {stats.Attack:F1} (+0.5)");
        Console.WriteLine($"방어력: {stats.Defense} (+ 1)");

        stats.Attack += 0.5f;
        stats.Defense += 1;
    }

    // enum 타입의 변수를 받아 string 으로 변환
    private string GetJobType(JobType type)
    {
        return type switch
        {
            JobType.Warrior => "전사",
            JobType.Mage => "마법사",
            JobType.Archer => "궁수",
            _ => "백수"
        };
    }

    // 아이템을 장착해 얻은 능력치를 string으로 변환
    private string GetItemStat(int itemStat)
    {
        if (itemStat == 0) return "";
        else return $"(+{itemStat})";
    }
}

public class Warrior : Player
{
    public Warrior(string Name, int jobType) : base(Name, jobType) { }

    public override void EnterDungeon()
    {
        Console.WriteLine($"\n{Name}이(가) 칼과 방패를 힘차게 두드리며 던전에 입장합니다.");
        Thread.Sleep(1000);
    }

    public override void Rest(int cost)
    {
        // 체력이 최대라면 휴식X
        if (stats.Hp == 100)
        {
            Console.WriteLine("\n지금은 어떤 몬스터든 해치울 수 있을 거 같습니다.");
            Thread.Sleep(1000);

            return;
        }

        else if (stats.Gold >= cost)
        {
            // 골드 차감
            stats.Gold -= cost;

            Console.WriteLine("\n무거운 갑옷과 무기를 내려놓고 잠에 듭니다.");
            Thread.Sleep(1000);

            stats.Hp += 30; // 체력 100으로 회복

            Console.WriteLine($"\n체력이 회복되었습니다!\n현재 체력 -> {stats.Hp}");
            Thread.Sleep(1000);

            return;
        }

        else
        {
            Console.WriteLine("\nGold가 부족합니다.");
            Thread.Sleep(1000);

            return;
        }
    }
}

public class Mage : Player
{
    public Mage(string Name, int jobType) : base(Name, jobType) { }

    public override void EnterDungeon()
    {
        Console.WriteLine($"\n{Name}이(가) 주문을 중얼중얼... 외우며 던전에 입장합니다.");
        Thread.Sleep(1000);
    }

    public override void Rest(int cost)
    {
        // 체력이 최대라면 휴식X
        if (stats.Hp == 100)
        {
            Console.WriteLine("\n지금은 어떤 마법이던 쓸 수 있을 거 같습니다.");
            Thread.Sleep(1000);

            return;
        }

        else if (stats.Gold >= cost)
        {
            // 골드 차감
            stats.Gold -= cost;

            Console.WriteLine("\n주문서와 지팡이를 내려놓고 잠에 듭니다.");
            Thread.Sleep(1000);

            stats.Hp += 30; // 체력 100으로 회복

            Console.WriteLine($"\n체력이 회복되었습니다!\n현재 체력 -> {stats.Hp}");
            Thread.Sleep(1000);

            return;
        }

        else
        {
            Console.WriteLine("\nGold가 부족합니다.");
            Thread.Sleep(1000);

            return;
        }
    }
}

public class Archer : Player
{
    public Archer(string Name, int jobType) : base(Name, jobType) { }

    public override void EnterDungeon()
    {
        Console.WriteLine($"\n{Name}이(가) 괜히 활시위를 한번 당겨보며 던전에 입장합니다.");
        Thread.Sleep(1000);
    }

    public override void Rest(int cost)
    {
        // 체력이 최대라면 휴식X
        if (stats.Hp == 100)
        {
            Console.WriteLine("\n지금은 화살을 대충 쏴도 다 맞을 거 같습니다.");
            Thread.Sleep(1000);

            return;
        }

        else if (stats.Gold >= cost)
        {
            // 골드 차감
            stats.Gold -= cost;

            Console.WriteLine("\n활과 화살 통을 내려놓고 잠에 듭니다.");
            Thread.Sleep(1000);

            stats.Hp += 30; // 체력 30 회복

            Console.WriteLine($"\n체력이 회복되었습니다!\n현재 체력 -> {stats.Hp}");
            Thread.Sleep(1000);

            return;
        }

        else
        {
            Console.WriteLine("\nGold가 부족합니다.");
            Thread.Sleep(1000);

            return;
        }
    }
}