using System;

namespace LotSystem.UI.Windows
{
    public static class WindowUtils
    {
        public static void SetTitle(this IWindow window)
        {
            Console.Title = window.Title;
        }

        public static void WriteTitle(this IWindow window)
        {
            Console.Clear();

            window.WriteSeparator();

            if (UIManager.Instance.CurrentSessionId.HasValue)
            {
                var oldPos = Console.CursorLeft;
                Console.CursorLeft = Console.WindowWidth - 37 - 9;

                Console.Write("Session: " + UIManager.Instance.CurrentSessionId.Value);

                Console.CursorLeft = oldPos;
            }

            Console.WriteLine(window.Title);
            Console.WriteLine();

            window.WriteSeparator();
        }

        public static void WriteSeparator(this IWindow window)
        {
            for (var i = 0; i < Console.WindowWidth; i++)
                Console.Write("=");

            Console.WriteLine();
        }

        public static int PromptSingleNumber(this IWindow window, string prompt, Func<int, bool> validator)
        {
            int intResponse;
            char? response = null;
            do
            {
                if (response is not null)
                {
                    Console.WriteLine($"\"{response}\" is not a option");
                    Console.ReadKey();
                    Console.WriteLine();
                    Console.CursorTop -= 3;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 100; j++)
                            Console.Write(" ");
                        Console.WriteLine();
                    }

                    Console.CursorTop -= 3;
                }

                Console.Write(prompt + ": ");

                response = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (!int.TryParse(response.ToString(), out intResponse))
                    continue;
            }
            while (!validator(intResponse));

            return intResponse;
        }

        public static string Prompt(this IWindow window, string prompt, Func<string, bool> validator)
        {
            string response = null;
            do
            {
                if (response is not null)
                {
                    Console.WriteLine($"\"{response}\" is not a response");
                    Console.ReadKey();
                    Console.WriteLine();
                    Console.CursorTop -= 3;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 100; j++)
                            Console.Write(" ");
                        Console.WriteLine();
                    }

                    Console.CursorTop -= 3;
                }

                Console.Write(prompt + ": ");

                response = Console.ReadLine();

            }
            while (!validator(response));

            return response;
        }

        public static string PromptPassword(this IWindow window, string prompt)
        {
            string response = null;
            do
            {
                if (response is not null)
                {
                    Console.WriteLine($"\"{response}\" is not a response");
                    Console.ReadKey();
                    Console.WriteLine();
                    Console.CursorTop -= 3;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 100; j++)
                            Console.Write(" ");
                        Console.WriteLine();
                    }

                    Console.CursorTop -= 3;
                }

                Console.Write(prompt + ": ");

                response = "";
                ConsoleKeyInfo lastKey;
                do
                {
                    lastKey = Console.ReadKey(true);
                    if(lastKey.Key == ConsoleKey.Enter)
                    {
                        break;
                    }

                    if(lastKey.Key == ConsoleKey.Backspace)
                    {
                        if (response.Length == 0)
                            continue;
                        response = response[..^1];
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                        continue;
                    }

                    Console.Write("*");
                    response += lastKey.KeyChar;
                }
                while (lastKey.Key != ConsoleKey.Enter);
                Console.WriteLine();
            }
            while (string.IsNullOrWhiteSpace(response));

            return response;
        }
    }
}
