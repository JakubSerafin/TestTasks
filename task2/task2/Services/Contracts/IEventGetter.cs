using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{



    public interface ISyncDate
    {
        DateTimeOffset? SyncDate { get; set; }
    }

    public interface IServiceBlocker
    {
        bool CanProcess();
    }

    public interface IAsyncEventGetter
    {
        Task<IList<EventObject>>  GetEventNotifications();
    }

    public interface IEventGetter
    {
        IList<EventObject> GetEventNotifications();
    }

    public interface IPagedEventGeter
    {
        IList<EventObject> GetPage(int pageNum, int pageSize);
    }
}
