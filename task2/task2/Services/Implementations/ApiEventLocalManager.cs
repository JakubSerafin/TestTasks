using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class ApiEventLocalManager : IEventGetter, IPagedEventGeter
    {
        private List<EventObject> _events = new List<EventObject>();
        private IAsyncEventGetter emptyCollectionReciever;
        private IAsyncEventGetter filledCollectionReciever;
        private ISyncDate lastSyncDateSeter;
        public ApiEventLocalManager(IAsyncEventGetter emptyCollectionReciever, IAsyncEventGetter filledCollectionReciever, ISyncDate lastSyncDateSeter)
        {
            this.emptyCollectionReciever = emptyCollectionReciever;
            this.filledCollectionReciever = filledCollectionReciever;
            this.lastSyncDateSeter = lastSyncDateSeter;
        }

        public IList<EventObject> GetEventNotifications()
        {
            return _events;
        }




        public async Task GetFromRemote()
        {
            IList<EventObject> events;
            if (lastSyncDateSeter.SyncDate == null)
            {
                events = await emptyCollectionReciever.GetEventNotifications();
            }
            else
            {
                events = await filledCollectionReciever.GetEventNotifications();
            }

            _events.AddRange(events);
            lastSyncDateSeter.SyncDate = DateTime.Now;
        }

        public IList<EventObject> GetPage(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }
    }


    //public class RealEventGetter: 
}
