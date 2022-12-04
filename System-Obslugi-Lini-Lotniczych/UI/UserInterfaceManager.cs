using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LotSystem.Logger.API;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI;

public sealed class UserInterfaceManager
{
    private static UserInterfaceManager instance;
    public static UserInterfaceManager Instance => instance ??= new UserInterfaceManager();

    private readonly Dictionary<string, IGWindow> _windows = new();

    private ILogger _logger;
    
    private readonly Stack<IGWindow> _windowStack = new();

    public Guid? CurrentSessionId { get; set; }
    private bool _enabled = true;

    public void Start(ILogger logger, Dictionary<Type, object> injectableTypes)
    {
        _logger = logger;

        RegisterWindows(injectableTypes);

        _windowStack.Push(_windows["__entry-point"]);
        _windowStack.Peek().Open();

        _enabled = true;
        Task.Run(GraphicsUserInterfaceLoop);
    }

    private void GraphicsUserInterfaceLoop()
    {
        while (_enabled)
        {
            try
            {
                var key = Console.ReadKey(true);
                var window = _windowStack.Peek();

                if (key.Key == ConsoleKey.Escape)
                {
                    CloseCurrentWindow();
                    continue;
                }

                window.OnKeyPressed(key);
            }
            catch (Exception ex)
            {
                _logger.Error($"{(_windowStack.TryPeek(out var w) ? w.Title : "NO WINDOW")} caused an exception");
                _logger.Error(ex);
            }
        }
    }

    private void RegisterWindows(Dictionary<Type, object> injectableTypes)
    {
        _windows.Clear();
        foreach (var type in GetType().Assembly.GetTypes()
                     .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IGWindow))))
        {
            var window = type.CreateInstance<IGWindow>(injectableTypes);

            _windows.Add(window.Id, window);
        }
    }

    public void OpenWindow(string window)
    {
        if (window is null)
        {
            throw new ArgumentNullException(nameof(window));
        }

        if (!_windows.TryGetValue(window, out var newWindow))
            throw new ArgumentException($"Window with name {window} not found", nameof(window));

        OpenWindow(newWindow);
    }

    public void OpenWindow(IGWindow window)
    {
        window.Console.Parent = _windowStack.Peek().Console;

        _windowStack.Push(window);

        window.Open();
    }

    public void CloseCurrentWindow()
    {
        _windowStack.Pop().Close();
        _windowStack.Peek().Resume();
    }
}