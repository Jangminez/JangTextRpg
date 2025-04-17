using System;
using System.Text;

namespace JangTextRpg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Green;

            Game game = new Game();
            
            game.StartGame();
        }
    }
}