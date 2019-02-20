using System.Threading.Tasks;

namespace Sonnar.Interfaces
{
    interface IRequestHandler
    {
        Task HandleRequest();
    }
}
