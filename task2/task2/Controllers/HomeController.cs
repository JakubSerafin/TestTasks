using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using task2.Models;
using task2.Models.Services.Contracts;

namespace task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventGetter _eventGetter;

        public HomeController(IEventGetter eventGetter)
        {
            _eventGetter = eventGetter;
        }

        public IActionResult Index()
        {
            var notifications = _eventGetter.GetEventNotifications();
            var model = new IndexModel
            {
                Entities = notifications.ToList()
            };

            return View(model);
        }

   
    }
}
