using System;

namespace LotSystem.UI.Windows.API;

public static class WindowUtils
{
    public static void SetTitle(this IWindow window)
    {
        Console.Title = window.Title;
    }

    public static void WriteTitle(this IWindow window)
    {
        Console.SetCursorPosition(0, 0);
        window.WriteSeparator();

        if (UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            var oldPos = window.Console.CursorLeft;
            window.Console.CursorLeft = Console.WindowWidth - 37 - 9;

            window.Console.Write("Session: " + UserInterfaceManager.Instance.CurrentSessionId.Value);

            window.Console.CursorLeft = oldPos;
        }

        window.Console.WriteLine(window.Title);
        window.Console.WriteLine();

        window.WriteSeparator();
    }

    public static void WriteSeparator(this IWindow window)
    {
        for (var i = 0; i < Console.WindowWidth; i++)
            window.Console.Write("=");

        window.Console.WriteLine();
    }

    public static int PromptSingleNumber(this IWindow window, string prompt, Func<int, bool> validator)
    {
        int intResponse;
        char? response = null;
        do
        {
            begin:
            if (response is not null)
            {
                window.Console.WriteLine($"\"{response}\" is not a option");
                window.Console.ReadKey();
                window.Console.WriteLine();
                window.Console.CursorTop -= 3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 100; j++)
                        window.Console.Write(" ");
                    window.Console.WriteLine();
                }

                window.Console.CursorTop -= 3;
            }

            window.Console.Write(prompt + ": ");

            response = window.Console.ReadKey().KeyChar;
            window.Console.WriteLine();

            if (!int.TryParse(response.ToString(), out intResponse))
                goto begin;
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

    private static PromptKeyResponse PromptHandleKey(IWindow window, out ConsoleKeyInfo key)
    {
        key = window.Console.ReadKey(true);
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

    private static void OnInvalidResponse(IWindow window, string response)
    {
        window.Console.WriteLine($"\"{response}\" is not a response");
        window.Console.ReadKey();
        window.Console.WriteLine();
        window.Console.CursorTop -= 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 100; j++)
                window.Console.Write(" ");
            window.Console.WriteLine();
        }

        window.Console.CursorTop -= 3;
    }

    private static string RemoveOneChar(IWindow window, string input)
    {
        window.Console.RemoveOneChar();

        return input[..^1];
    }

    public static string Prompt(this IWindow window, string prompt, Func<string, bool> validator)
    {
        string response = null;
        do
        {
            if (response is not null)
                OnInvalidResponse(window, response);

            window.Console.Write(prompt + ": ");

            response = window.Console.ReadLine();

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
                OnInvalidResponse(window, response);

            window.Console.Write(prompt + ": ");

            response = "";
            ConsoleKeyInfo lastKey;
            PromptKeyResponse result;
            do
            {
                result = PromptHandleKey(window, out lastKey);
                switch (result)
                {
                    case PromptKeyResponse.REMOVE_ONE:
                        if (response.Length == 0)
                            continue;

                        response = RemoveOneChar(window, response);
                        continue;

                    case PromptKeyResponse.CLEAR:
                        while (response.Length > 0)
                            response = RemoveOneChar(window, response);
                        continue;

                    case PromptKeyResponse.KEY:
                        window.Console.Write("*");
                        response += lastKey.KeyChar;
                        continue;
                }
            }
            while (result != PromptKeyResponse.SUBMIT);

            window.Console.WriteLine();
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
                OnInvalidResponse(window, response);

            window.Console.Write(prompt + ": ");

            response = "";
            ConsoleKeyInfo lastKey;
            PromptKeyResponse result;
            do
            {
                result = PromptHandleKey(window, out lastKey);
                switch (result)
                {
                    case PromptKeyResponse.REMOVE_ONE:
                        if (response.Length == 0)
                            continue;

                        if (response.Length % 3 == 0 && response.Length < 9)
                        {
                            window.Console.CursorLeft--;
                            window.Console.Write(" ");
                            window.Console.CursorLeft--;
                        }

                        response = response[..^1];
                        window.Console.CursorLeft--;
                        window.Console.Write(" ");
                        window.Console.CursorLeft--;
                        continue;

                    case PromptKeyResponse.CLEAR:
                        while (response.Length > 0)
                        {
                            if (response.Length % 3 == 0 && response.Length < 9)
                            {
                                window.Console.CursorLeft--;
                                window.Console.Write(" ");
                                window.Console.CursorLeft--;
                            }

                            response = response[..^1];
                            window.Console.CursorLeft--;
                            window.Console.Write(" ");
                            window.Console.CursorLeft--;
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

                                window.Console.Write(lastKey.KeyChar);
                                response += lastKey.KeyChar;

                                if (response.Length % 3 == 0 && response.Length < 9)
                                    window.Console.Write("-");
                                break;
                        }
                        break;

                    case PromptKeyResponse.SUBMIT:
                        if (response.Length != 9)
                        {
                            result = PromptKeyResponse.KEY;
                        }

                        break;
                }
            }
            while (result != PromptKeyResponse.SUBMIT);

            window.Console.WriteLine();
        }
        while (string.IsNullOrWhiteSpace(response));

        return response;
    }
}