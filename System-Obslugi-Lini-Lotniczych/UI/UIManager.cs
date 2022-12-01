using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LotSystem.Logger.API;
using LotSystem.UI.Windows;

namespace LotSystem.UI
{
    public sealed class UIManager
    {
        private static UIManager instance;
        public static UIManager Instance => instance ??= new UIManager();

        public readonly Dictionary<string, IWindow> Windows = new();

        private ILogger _logger;

        private IWindow _defaultWindow; 
        private IWindow _currentWindow;
        private IWindow _targetWindow;
        private Task _uiThread;
        public Guid? CurrentSessionId { get; set; }
        private bool _enabled = true;

        public void Start(ILogger logger, Dictionary<Type, object> injectableTypes)
        {
            _logger = logger;

            RegisterWindows(injectableTypes);

            _targetWindow = _defaultWindow = Windows["default"];

            _enabled = true;
            _uiThread = UILoop();
        }

        private async Task UILoop()
        {
            while(_enabled)
            {
                await Task.Delay(100);

                if(_currentWindow != _targetWindow)
                {
                    _currentWindow?.Close();
                    _targetWindow.Open();
                    _currentWindow = _targetWindow;
                }
                else if(_currentWindow is null)
                {
                    _currentWindow = _targetWindow = _defaultWindow;
                    _currentWindow.Open();
                }

                try
                {
                    await _currentWindow.Update();
                }
                catch(Exception ex)
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
                _targetWindow = _defaultWindow;
                return;
            }

            if (!Windows.TryGetValue(window, out var newWindow))
                throw new ArgumentException($"Window with name {window} not found", nameof(window));

            _targetWindow = newWindow;
        }
    }
}
