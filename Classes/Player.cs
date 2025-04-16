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
        set => hp = Math.Max(0, value);
    }

    private int attack;
    public int Attack
    {
        get => attack;
        set => attack = Math.Max(0, value);
    }

    private int defense;
    public int Defense
    {
        get => defense;
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

public class Player
{
    private JobType jobType { get; } // 플레이어 직업
    public string Name { get; } // 플레이어 이름
    public Stats stats; // 플레이어 능력치
    
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
        stats.Gold = 15000;

    }

    // 상태창 페이지
    public void StatusPage()
    {
        while (true)
        {
            // 콘솔 창 클리어 및 캐릭터 정보 표시
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시 됩니다.\n");
            Console.WriteLine($"닉네임: {Name}");
            Console.WriteLine($"Lv. {stats.Level:00}");
            Console.WriteLine($"Chad ({GetJobType(jobType)})");
            Console.WriteLine($"공격력 : {stats.Attack} " + GetItemStat(stats.ItemAttack));
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

    // 나중에 던전 입장 시에 사용
    virtual public void EnterDungeon()
    {
        Console.WriteLine($"\n{Name}이(가) 던전에 입장합니다.");
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
        Console.WriteLine($"{Name}이(가) 칼과 방패를 힘차게 두드리며 던전에 입장합니다.");
    }
}

public class Mage : Player
{
    public Mage(string Name, int jobType) : base(Name, jobType) { }

    public override void EnterDungeon()
    {
        Console.WriteLine($"{Name}이(가) 주문을 중얼중얼...외우며 던전에 입장합니다.");
    }
}

public class Archer : Player
{
    public Archer(string Name, int jobType) : base(Name, jobType) { }

    public override void EnterDungeon()
    {
        Console.WriteLine($"{Name}이(가) 괜히 활시위를 한번 당겨보며 던전에 입장합니다.");
    }
}