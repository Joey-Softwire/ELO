﻿using System.Linq;
using System.Web.Mvc;
using EloWeb.Models;

namespace EloWeb.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        public ActionResult Index()
        {
            ViewData.Model = Games.All();
            return View();
        }

        // GET: Games/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = new ViewModels.CreateGame { Players = Players.Names().OrderBy(p=>p) };
            return View();
        }

        // POST: Games/Create
        [HttpPost]
        public ActionResult Create(Game game)
        {
            Games.Add(game);
            Players.UpdateRatings(game);
            return Redirect("/Games");
        }

    }
}