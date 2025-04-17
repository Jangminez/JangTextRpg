public class Game
{
    public static bool isGameOver = false;
    private Player player;  // 플레이어 객체
    private Shop shop = new Shop();
    private Pages pages = new Pages();

    // 자주쓰이는 string 상수로 저장
    public const string CHOICE_ACTION = "원하시는 행동을 선택하세요.";
    public const string WRONG_CHOICE = "잘못된 입력입니다. 다시 입력해주세요.";

    //Main에서 Game 클래스 객체 생성 후 메서드 실행
    public void StartGame()
    {
        if (!isGameOver)
        {
            CreatePlayer();
        }
    }

    // 게임의 메인 루프
    void MainGame()
    {
        // 아이템 초기화 (초기에 한 번만 생성됨)
        ItemManager.Instance.CreateItem();

        while (!isGameOver)
        {
            Console.Clear();
            Console.WriteLine("당신은 현재 마을에 있습니다.");

            // 행동 동작 입력 대기
            int choice = InputValidator("1. 상태보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 휴식하기", CHOICE_ACTION, 1, 3);

            switch (choice)
            {
                case 1:
                    player.StatusPage(); // 상태창으로 이동
                    break;

                case 2:
                    pages.InventoryPage(player); // 인벤토리 페이지 이동
                    break;

                case 3:
                    shop.ShopMainPage(player); // 상점 페이지 이동
                    break;

                case 4:
                    // 던전 입장
                    break;

                case 5:
                    // 휴식하기
                    break;

            }
        }
    }

    // 플레이어 생성
    private void CreatePlayer()
    {
        while (true)
        {
            bool isNameSet = false;
            bool isJobSet = false;

            // 이름과 직업 설정
            // 콜백을 받아 bool 타입 변수 값 대입
            string name = SetPlayerName(success => isNameSet = success);
            int jobType = SetPlayerJob(success => isJobSet = success);

            // 이름과 직업이 잘 설정되었으면 player 생성 후 MainGame으로 이동 아니면 다시 설정
            if (isNameSet && isJobSet)
            {
                switch (jobType)
                {
                    case 1: // 전사
                        player = new Warrior(name, jobType);
                        break;

                    case 2: // 마법사
                        player = new Mage(name, jobType);
                        break;

                    case 3: // 궁수
                        player = new Archer(name, jobType);
                        break;
                }

                MainGame();
                break;
            }
        }
    }

    // 플레이어 이름 설정
    private string SetPlayerName(Action<bool> onSet)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다!!");

            // 이름 입력 유효성 검사
            string playerName = InputValidator(2, 10);

            Console.WriteLine($"\n입력하신 이름은 \"{playerName}\"입니다.\n");
            Console.WriteLine("정말로 이 이름을 사용하시겠습니까?");

            // 선택 입력 유효성 검사
            int choice = InputValidator("1. 네\n2. 아니오", CHOICE_ACTION, 1, 2);

            switch (choice)
            {
                case 1:
                    onSet?.Invoke(true); // 설정에 성공, 콜백 호출
                    return playerName;

                case 2:
                    onSet?.Invoke(false); // 설정에 실패, 콜백 호출
                    break;
            }
        }
    }

    // 플레이어 직업 설정
    private int SetPlayerJob(Action<bool> onSet)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다!!");

            Console.WriteLine("\n원하시는 직업을 고르세요.");
            // 선택 입력 유효성 검사
            int jobType = InputValidator("1. 전사\n2. 마법사\n3. 궁수", "원하시는 직업의 번호를 선택하세요.", 1, 3);

            Console.WriteLine($"\n선택하신 직업은 \"{(JobType)jobType}\"입니다.\n");
            Console.WriteLine("이 직업을 선택하시겠습니까?");

            // 선택 입력 유효성 검사
            int choice = InputValidator("1. 네\n2. 아니오", CHOICE_ACTION, 1, 2);

            switch (choice)
            {
                case 1:
                    onSet?.Invoke(true); // 설정에 성공, 콜백 호출
                    return jobType;

                case 2:
                    onSet?.Invoke(false); // 설정에 실패, 콜백 호출
                    break;
            }
        }
    }

    // 플레이어 응답 대기 메서드 (행동 선택)
    public static int InputValidator(string options, string msg, int min, int max)
    {
        while (true)
        {
            Console.Write($"\n{options}\n\n{msg}\n>> ");
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"{WRONG_CHOICE}\n");
        }
    }

    // 플레이어 응답 대기 메서드 (이름 입력)
    public static string InputValidator(int minLength, int maxLength)
    {
        while (true)
        {
            Console.Write($"\n원하시는 이름을 입력해주세요.(길이{minLength} ~ {maxLength})\n>> ");
            string inputName = Console.ReadLine();

            // 이름 길이가 조건에 맞다면 return
            if (inputName.Length >= minLength && inputName.Length <= maxLength) return inputName;

            Console.WriteLine($"{WRONG_CHOICE}");
        }
    }
}