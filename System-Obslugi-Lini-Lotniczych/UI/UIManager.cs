using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LotSystem.UI.Windows;

namespace LotSystem.UI
{
    public sealed class UIManager
    {
        private static UIManager instance;
        public static UIManager Instance => instance ??= new UIManager();

        public readonly Dictionary<string, IWindow> Windows = new();

        private IWindow _defaultWindow;
        private IWindow _currentWindow;
        private IWindow _targetWindow;
        private Thread _uiThread;
        public Guid? CurrentSessionId { get; set; }
        private bool _enabled = true;

        public void Start(Dictionary<Type, object> injectableTypes)
        {
            RegisterWindows(injectableTypes);

            _enabled = true;
            _uiThread = new Thread(UILoop);

            _targetWindow = _defaultWindow = Windows["default"];

            _uiThread.Start();
        }

        private void UILoop()
        {
            while(_enabled)
            {
                Thread.Sleep(100);

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

                _currentWindow.Update();
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
