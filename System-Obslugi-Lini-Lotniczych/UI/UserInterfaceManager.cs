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

    public readonly Dictionary<string, IWindow> Windows = new();

    private ILogger _logger;

    private IWindow _defaultWindow;
    private readonly Stack<IWindow> _windowStack = new();
    private Task _uiThread;
    public Guid? CurrentSessionId { get; set; }
    private bool _enabled = true;

    public void Start(ILogger logger, Dictionary<Type, object> injectableTypes)
    {
        _logger = logger;

        RegisterWindows(injectableTypes);

        _defaultWindow = Windows["default"];
        OpenWindow(_defaultWindow);

        _enabled = true;
        _uiThread = UserInterfaceLoop();
    }

    private async Task UserInterfaceLoop()
    {
        while (_enabled)
        {
            await Task.Delay(100);

            try
            {
                await _windowStack.Peek().Update();
            }
            catch(ReturnToParentWindowException)
            {
                _windowStack.Pop().Close();
                if (_windowStack.Peek().PreserveContentOnTransferControl)
                    _windowStack.Peek().Console.RewriteScreen();
                else
                    _windowStack.Peek().Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }

    private void RegisterWindows(Dictionary<Type, object> injectableTypes)
    {
        Windows.Clear();
        foreach (var type in GetType().Assembly.GetTypes()
                     .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IWindow))))
        {
            var window = type.CreateInstance<IWindow>(injectableTypes);

            Windows.Add(window.Id, window);
        }
    }

    public void OpenWindow(string window)
    {
        if (window is null)
        {
            throw new ArgumentNullException(nameof(window));
        }

        if (!Windows.TryGetValue(window, out var newWindow))
            throw new ArgumentException($"Window with name {window} not found", nameof(window));

        OpenWindow(newWindow);
    }

    public void OpenWindow(IWindow window)
    {
        window.Open();
        _windowStack.Push(window);
    }
}