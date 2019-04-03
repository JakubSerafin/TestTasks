using System.Collections.Generic;
using System.Threading.Tasks;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{
    public interface IAsyncEventGetter
    {
        Task<IList<EventObject>>  GetEventNotifications();
    }
}
