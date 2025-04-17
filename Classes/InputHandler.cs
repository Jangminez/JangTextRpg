public static class InputHandler
{
    // 자주쓰이는 string 상수로 저장
    public const string CHOICE_ACTION = "원하시는 행동을 선택하세요.";
    public const string WRONG_INPUT = "잘못된 입력입니다. 다시 입력해주세요.";

     // 플레이어 응답 대기 메서드 (행동 선택)
    public static int InputValidator(string options, int min, int max)
    {
        while (true)
        {
            Console.Write($"\n{options}\n\n{CHOICE_ACTION}\n>> ");
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"{WRONG_INPUT}\n");
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

            Console.WriteLine($"{WRONG_INPUT}");
        }
    }
}