using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using task2.Models.Services.Contracts;
using task2.Models.Services.Implementations;
using Xunit;

namespace test2.tests
{

    public class EventsGetterBacgroundServiceTests
    {
        [Fact]
        public void Run_IsCallingGetterAfterCalled()
        {
            bool actionWasCalled = false;
            Action action= () =>{actionWasCalled = true;};
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var getter = new EventsGetterBacgroundService(action, cancellationTokenSource.Token);
            var task = getter.Run();
            cancellationTokenSource.Cancel();
            task.Wait();

            Assert.True(actionWasCalled);

        }

        [Fact]
        public void Run_StopWhenTokenIsSetToCancelWhileWaitingOnUnlock()
        {
                var blocker = Substitute.For<IServiceBlocker>();
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                blocker.CanProcess().Returns(false);

                var getter = new EventsGetterBacgroundService(() => { }, cancellationTokenSource.Token, blocker);

                var task = getter.Run();
                cancellationTokenSource.Cancel();
                task.Wait(100);
                Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public void Run_IsCallingGetterWhenBlockerUnlock()
        {
            
            bool actionWasCalled = false;
            Action action = () => { actionWasCalled = true; };
            var blocker = Substitute.For<IServiceBlocker>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            blocker.CanProcess().Returns(false);

            var getter = new EventsGetterBacgroundService(action, cancellationTokenSource.Token, blocker);
            
            var task = getter.Run();
            Assert.False(actionWasCalled);
            blocker.CanProcess().Returns(true);
            cancellationTokenSource.Cancel();
            task.Wait();
            Assert.True(actionWasCalled);
        }
    }

    public class ApiEventLocalManagerTests
    {
        [Fact]
        public void CacheIsEmpty_CallIEventGetter()
        {
            //var eventManager = new EventManager();
            //eventManager.Get
        }

        [Fact]
        public void CacheHasItems_CallForEventFromLastUpdate()
        {

        }
    }
}
