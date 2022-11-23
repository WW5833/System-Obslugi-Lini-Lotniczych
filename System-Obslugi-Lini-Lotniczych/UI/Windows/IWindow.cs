using System.Threading.Tasks;

namespace LotSystem.UI.Windows
{
    public interface IWindow
    {
        string Id { get; }
        string Title { get; }
        void Open();
        void Close();

        Task Update();
    }
}
