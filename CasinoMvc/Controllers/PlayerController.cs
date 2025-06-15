using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasinoMvc.Data;
using CasinoMvc.Models;

namespace CasinoMvc.Controllers
{
    public class PlayerController : Controller
    {
        private readonly CasinoContext _context;

        public PlayerController(CasinoContext context)
        {
            _context = context;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetInt32("LoggedId") == null)
            {
                return RedirectToAction("Login");
            }
            int? userId = (int)HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            Player player = await _context.Player.FindAsync((int)userId);
            return View(player);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult Register(Player player,IFormFile Avatar)
        {
            if (player == null)
            {
                return View();
            }
            player.Balance = 100;
            player.TotalWins = 0;
            player.TotalBets = 0;
            player.CreatedAt = DateTime.Now;
            player.LastBetAt = DateTime.Now;
            using (MemoryStream ms = new MemoryStream())
            {
                Avatar.CopyTo(ms);
                string base64 = Convert.ToBase64String(ms.ToArray());
                player.Avatar = base64;
            }
            _context.Player.Add(player);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(string NickName , string Password)
        {
            Player fromDb = _context.Player.FirstOrDefault(p => p.NickName == NickName && p.Password == Password);
            if (fromDb != null)
            {
                HttpContext.Session.SetInt32("LoggedId",fromDb.Id);
                HttpContext.Session.SetString("LoggedNick",fromDb.NickName);
                fromDb.IsActive = true;
                fromDb.LastBetAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }  
}
