using System;

namespace ConsoleLibrary
{
    public static class ConsoleMethods
    {
        public static void AddBlankLine()
        {
            Console.WriteLine();
        }

        public static void CenterText(string text)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.WriteLine(text);
        }

        public static void SetTitle(string title)
        {
            Console.Title = title;
            Console.Clear();

            CenterText(title);
            CenterText(new string('-', title.Length));
        }

        public static void WaitForKeypress(string message = "Press any key to continue...")
        {
            Console.WriteLine();
            CenterText(message);
            Console.ReadKey(true);
        }
    }
}
