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

    private readonly Dictionary<string, IWindow> _windows = new();

    private ILogger _logger;

    private IWindow _defaultWindow;
    private readonly Stack<IWindow> _windowStack = new();

    public Guid? CurrentSessionId { get; set; }
    private bool _enabled = true;

    public void Start(ILogger logger, Dictionary<Type, object> injectableTypes)
    {
        _logger = logger;

        RegisterWindows(injectableTypes);

        _defaultWindow = _windows["default"];
        OpenWindow(_defaultWindow);

        _enabled = true;
        _ = UserInterfaceLoop();
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
        _windows.Clear();
        foreach (var type in GetType().Assembly.GetTypes()
                     .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IWindow))))
        {
            var window = type.CreateInstance<IWindow>(injectableTypes);

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

    private void OpenWindow(IWindow window)
    {
        window.Open();
        _windowStack.Push(window);
    }
}