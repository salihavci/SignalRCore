using SignalRCore.API.Models;
using System.Threading.Tasks;

namespace SignalRCore.API.Hubs
{
    public interface IProductHub
    {
        Task ReceiveProduct(Product p);
    }
}
