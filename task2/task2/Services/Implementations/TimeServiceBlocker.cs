using System.Threading;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class TimeServiceBlocker : IServiceBlocker
    {
        private AutoResetEvent _canProcess = new AutoResetEvent(false);
        public TimeServiceBlocker()
        {

            Timer timer = new Timer((tim)=>_canProcess.Set(),null,0,30000);
            
        }

        bool IServiceBlocker.WaitCanProcess()
        {
            if (_canProcess.WaitOne())
            {
                return true;
            }
            return false;
        }
    }


    //public class RealEventGetter: 
}
