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

        private enum PromptKeyResponse
        {
            KEY,
            SUBMIT,
            REMOVE_ONE,
            CLEAR,
        }

        private static PromptKeyResponse PromptHandleKey(out ConsoleKeyInfo key)
        {
            key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return PromptKeyResponse.SUBMIT;

                case ConsoleKey.Backspace:
                    return PromptKeyResponse.REMOVE_ONE;
                case ConsoleKey.Clear:
                    return PromptKeyResponse.CLEAR;

                case ConsoleKey.Escape:
                    throw new ReturnToParentWindowException();

                default:
                    return PromptKeyResponse.KEY;
            }
        }

        internal sealed class ReturnToParentWindowException : Exception
        {
        }

        private static void OnInvalidResponse(string response)
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

        private static string RemoveOneChar(string input)
        {
            Console.CursorLeft--;
            Console.Write(" ");
            Console.CursorLeft--;

            return input[..^1];
        }

        public static string Prompt(this IWindow window, string prompt, Func<string, bool> validator)
        {
            string response = null;
            do
            {
                if (response is not null)
                    OnInvalidResponse(response);

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
                    OnInvalidResponse(response);

                Console.Write(prompt + ": ");

                response = "";
                ConsoleKeyInfo lastKey;
                PromptKeyResponse result;
                do
                {
                    result = PromptHandleKey(out lastKey);
                    switch (result)
                    {
                        case PromptKeyResponse.REMOVE_ONE:
                            if (response.Length == 0)
                                continue;

                            response = RemoveOneChar(response);
                            continue;

                        case PromptKeyResponse.CLEAR:
                            while (response.Length > 0)
                                response = RemoveOneChar(response);
                            continue;

                        case PromptKeyResponse.KEY:
                            Console.Write("*");
                            response += lastKey.KeyChar;
                            continue;
                    }
                }
                while (result != PromptKeyResponse.SUBMIT);

                Console.WriteLine();
            }
            while (string.IsNullOrWhiteSpace(response));

            return response;
        }

        public static string PromptPhoneNumber(this IWindow window, string prompt)
        {
            string response = null;
            do
            {
                if (response is not null)
                    OnInvalidResponse(response);

                Console.Write(prompt + ": ");

                response = "";
                ConsoleKeyInfo lastKey;
                PromptKeyResponse result;
                do
                {
                    result = PromptHandleKey(out lastKey);
                    switch (result)
                    {
                        case PromptKeyResponse.REMOVE_ONE:
                            if (response.Length == 0)
                                continue;

                            if (response.Length % 3 == 0 && response.Length < 9)
                            {
                                Console.CursorLeft--;
                                Console.Write(" ");
                                Console.CursorLeft--;
                            }

                            response = response[..^1];
                            Console.CursorLeft--;
                            Console.Write(" ");
                            Console.CursorLeft--;
                            continue;

                        case PromptKeyResponse.CLEAR:
                            while (response.Length > 0)
                            {
                                if (response.Length % 3 == 0 && response.Length < 9)
                                {
                                    Console.CursorLeft--;
                                    Console.Write(" ");
                                    Console.CursorLeft--;
                                }

                                response = response[..^1];
                                Console.CursorLeft--;
                                Console.Write(" ");
                                Console.CursorLeft--;
                            }
                            continue;

                        case PromptKeyResponse.KEY:
                            switch (lastKey.Key)
                            {
                                case ConsoleKey.D0:
                                case ConsoleKey.NumPad0:
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D2:
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D3:
                                case ConsoleKey.NumPad3:
                                case ConsoleKey.D4:
                                case ConsoleKey.NumPad4:
                                case ConsoleKey.D5:
                                case ConsoleKey.NumPad5:
                                case ConsoleKey.D6:
                                case ConsoleKey.NumPad6:
                                case ConsoleKey.D7:
                                case ConsoleKey.NumPad7:
                                case ConsoleKey.D8:
                                case ConsoleKey.NumPad8:
                                case ConsoleKey.D9:
                                case ConsoleKey.NumPad9:
                                    if (response.Length >= 9)
                                        continue;

                                    Console.Write(lastKey.KeyChar);
                                    response += lastKey.KeyChar;

                                    if (response.Length % 3 == 0 && response.Length < 9)
                                        Console.Write("-");
                                    break;
                            }
                            break;
                    }
                }
                while (result != PromptKeyResponse.SUBMIT);
                
                Console.WriteLine();
            }
            while (string.IsNullOrWhiteSpace(response));

            return response;
        }
    }
}
