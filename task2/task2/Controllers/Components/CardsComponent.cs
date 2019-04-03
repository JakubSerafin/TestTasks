using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using task2.Models.Services.Contracts;

namespace task2.Controllers.Components
{
    public class CardsViewComponent : ViewComponent
    {
        private IEventGetter _eventGetter;

        public CardsViewComponent(IEventGetter eventGetter)
        {
            _eventGetter = eventGetter;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            var items = await Task.Run(() =>
            {

                var collection = _eventGetter.GetEventNotifications();
                while (collection.Count == 0)
                {
                    collection = _eventGetter.GetEventNotifications();

                }

                return collection;
            });
            return View(items);
        }

        
    }
}
