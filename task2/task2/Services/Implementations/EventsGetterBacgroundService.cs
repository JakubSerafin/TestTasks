﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Hosting;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{

    public interface IActionProvider
    {
        void Run();
    }

    public class EventsGetterBacgroundServiceWrapper : IHostedService
    {
        private IServiceBlocker _blocker;
        private Action _action;
        private EventsGetterBacgroundService _backgroundService;
        private Task _backgroundServiceTask;

        public EventsGetterBacgroundServiceWrapper(IServiceBlocker blocker, IActionProvider action)
        {
            _blocker = blocker;
            _action = action.Run;
            _backgroundService = new EventsGetterBacgroundService(_action, _blocker);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            _backgroundServiceTask = _backgroundService.Run(cancellationToken);
            return _backgroundServiceTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class EventsGetterBacgroundService
    {
        private readonly Action _actionToCall;
        private readonly IServiceBlocker _blocker;
        private CancellationToken _token;


        public EventsGetterBacgroundService(Action action,  IServiceBlocker blocker = null)
        {
            _actionToCall = action;
;
            _blocker = blocker;

        }

        public async Task Run(CancellationToken token)
        {
            _token = token;
            await Task.Run(() =>
            {
                while (!_token.IsCancellationRequested)
                {
                    if (_blocker?.CanProcess() ?? true)
                    {
                        _actionToCall.Invoke();
                    }
                }
            });
        }
    }


    //public class RealEventGetter: 
}
