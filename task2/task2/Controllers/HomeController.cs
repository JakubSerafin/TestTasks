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
            
            var model = new IndexModel
            {
                //Entities = notifications.ToList()
            };

            return View(model);
        }


        public IActionResult GetCards()
        {
            var notifications = _eventGetter.GetEventNotifications();
            var entities = notifications.ToList();
            if (entities.Any())
            {
                return PartialView("CardsView", entities);

            }
            else
            {
                return NoContent();
            }
        }

        public IActionResult GetNewCards(long dateTime)
        {
            var notifications =  _eventGetter.GetEventNotifications();
            var entities = notifications.Where(e=>e.createDate> DateTimeOffset.FromUnixTimeMilliseconds(dateTime)).ToList();
            if (entities.Any())
            {
                return PartialView("CardsView", entities);

            }
            else
            {
                return NoContent();
            }
        }


    }
}
