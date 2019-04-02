using System.Threading;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class TimeServiceBlocker : IServiceBlocker
    {
        private bool _canProcess = false;
        public TimeServiceBlocker()
        {
            Timer timer = new Timer((tim)=>_canProcess=true,null,0,30000);
        }

        public bool CanProcess()
        {
            if (_canProcess == true)
            {
                _canProcess = false;
                return true;
            }

            return false;
        }
    }


    //public class RealEventGetter: 
}
