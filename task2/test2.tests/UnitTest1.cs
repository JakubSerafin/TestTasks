using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;
using task2.Models.Services.Implementations;
using Xunit;

namespace test2.tests
{

    public class EventsGetterBacgroundServiceTests
    {

        CancellationTokenSource _cancelationTokenSource = new CancellationTokenSource();
        private IServiceBlocker _standardBlocker = Substitute.For<IServiceBlocker>();


        private EventsGetterBacgroundService BuildEventsGetterBacgroundService(Func<Task> action, IServiceBlocker blocker = null)
        {
            return new EventsGetterBacgroundService(action, blocker);
        }

        [Fact]
        public void Run_IsCallingGetterAfterCalled()
        {
            bool actionWasCalled = false;
            Func<Task> action = () =>
            {
                actionWasCalled = true;
                return Task.CompletedTask;
            };

            var getter = BuildEventsGetterBacgroundService(action);
            var task = getter.Run(_cancelationTokenSource.Token);
            _cancelationTokenSource.CancelAfter(50); //this is bad, i know. To be replaced with something else. 
            task.Wait();

            Assert.True(actionWasCalled);

        }

        [Fact]
        public void Run_StopWhenTokenIsSetToCancelWhileWaitingOnUnlock()
        {

            var getter = BuildEventsGetterBacgroundService(() => Task.CompletedTask);

            var task = getter.Run(_cancelationTokenSource.Token);
                _cancelationTokenSource.Cancel();
                task.Wait(100);
                Assert.True(task.IsCompletedSuccessfully);
        }



        [Fact]
        public void Run_IsCallingGetterWhenBlockerUnlock()
        {
            
           

            bool actionWasCalled = false;
            Func<Task> action = () =>
            {
                actionWasCalled = true;
                return Task.CompletedTask;
            };

            var getter = BuildEventsGetterBacgroundService(action);

            _standardBlocker.WaitCanProcess().Returns(false);
            
            var task = getter.Run(_cancelationTokenSource.Token);
            Assert.False(actionWasCalled);
            _standardBlocker.WaitCanProcess().Returns(true);
            _cancelationTokenSource.Cancel();
            task.Wait();
            Assert.True(actionWasCalled);
        }
    }
    internal class MockSyncDate : ISyncDate
    {
        public DateTimeOffset? SyncDate { get; set; }
    }

    public class ApiEventLocalManagerTests
    {
        private IAsyncEventGetter _emptyCollectionReciever = Substitute.For<IAsyncEventGetter>();
        private IAsyncEventGetter _filledCollectionReciever = Substitute.For<IAsyncEventGetter>();
        private ISyncDate _dateSync = new MockSyncDate();


        [Fact]
        public async void GetFromRemote_CacheIsEmpty_CallIEventGetter()
        {

            _dateSync.SyncDate =  null;

            var eventsLocalManager = GetApiEventLocalManager();
            await eventsLocalManager.Run();
            await _emptyCollectionReciever.Received(1).GetEventNotifications();
            await _filledCollectionReciever.Received(0).GetEventNotifications();
        }

        private ApiEventLocalManager GetApiEventLocalManager()
        {
            return new ApiEventLocalManager(_emptyCollectionReciever, _filledCollectionReciever, _dateSync);
        }

        [Fact]
        public async void GetFromRemote_CacheHasItems_CallForEventFromLastUpdate()
        {
            _dateSync.SyncDate = DateTime.Parse("10.10.2010");

            var eventsLocalManager = GetApiEventLocalManager();
            await eventsLocalManager.Run();
            await _emptyCollectionReciever.Received(0).GetEventNotifications();
            await _filledCollectionReciever.Received(1).GetEventNotifications();
        }

        [Fact]
        public async void GetFromRemote_TwoCalls_CallBothGetters()
        {

            _dateSync.SyncDate = null;

            var eventsLocalManager = GetApiEventLocalManager();
            await eventsLocalManager.Run();
            await eventsLocalManager.Run();
            await _emptyCollectionReciever.Received(1).GetEventNotifications();
            await _filledCollectionReciever.Received(1).GetEventNotifications();
        }



        [Fact]
        public async void GetFromRemote_CacheEmpty_IsFillingCollection()
        {
            _dateSync.SyncDate =  null;
            IList<EventObject> data = new List<EventObject> {new EventObject(), new EventObject()};
            _emptyCollectionReciever.GetEventNotifications().Returns(Task.Run(()=>data));

            var eventsLocalManager = GetApiEventLocalManager();
            await eventsLocalManager.Run();

            Assert.Equal(data,eventsLocalManager.GetEventNotifications());
        }

        [Fact]
        public async void GetFromRemote_CacheNotEmpty_IsAppendToCollection()
        {
            _dateSync.SyncDate = null;
            IList<EventObject> data = new List<EventObject> { new EventObject(), new EventObject() };
            _emptyCollectionReciever.GetEventNotifications().Returns(Task.Run(() => data));
            IList<EventObject> data2 = new List<EventObject> { new EventObject(), new EventObject() };
            _filledCollectionReciever.GetEventNotifications().Returns(Task.Run(() => data2));


            var eventsLocalManager = GetApiEventLocalManager();
            await eventsLocalManager.Run(); //initialization
            await eventsLocalManager.Run(); // geting new items 

            Assert.Collection(eventsLocalManager.GetEventNotifications(),
                e => Assert.Equal(data[0],e),
                e => Assert.Equal(data[1], e),
                e => Assert.Equal(data2[0], e),
                e => Assert.Equal(data2[1], e)
                );
        }
    }
}
