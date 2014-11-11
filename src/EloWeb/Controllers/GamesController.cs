﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EloWeb.Models;
using EloWeb.Services;
using EloWeb.ViewModels;

namespace EloWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly Games _games;
        private readonly Players _players;
        private readonly Ratings _ratings;

        public GamesController(Games games, Players players, Ratings ratings)
        {
            _games = games;
            _players = players;
            _ratings = ratings;
        }

        // GET: Games
        public ActionResult Index()
        {
            var leaderboard = _players.All().OrderByDescending(p => p.CurrentRating);
            if (!leaderboard.Any())
                return Redirect("~/Players/NewLeague");

            ViewData.Model = _games.MostRecent(20);
            return View();
        }

        // GET: Games/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model =  new CreateGame
            {                
                Players = GetPlayerSelectList(),
                RecentGames = _games.MostRecent(10)
            };
            return View();
        }

        private IEnumerable<SelectListItem> GetPlayerSelectList()
        {
            var players = _players.Active();

            var selectList = players.OrderBy(p => p.Name).Select(p => 
                new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name
                }
            );

            return new SelectList(selectList, "Value", "Text");
        }

        // POST: Games/Create
        [HttpPost]
        public ActionResult Create(GameOutcome gameOutcome)
        {
            if (gameOutcome.WinnerId != gameOutcome.LoserId)
            {                         
                var winner = _players.Get(gameOutcome.WinnerId);
                var loser = _players.Get(gameOutcome.LoserId);
                var game = _games.Add(new Game(winner, loser));
                _ratings.UpdatePlayerRatings(game);
            }

            return Redirect("~/");
        }

    }

    public class GameOutcome
    {
        public long WinnerId { get; set; }
        public long LoserId { get; set; }
    }
}