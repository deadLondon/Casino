using CasinoMvc.Data;
using CasinoMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CasinoMvc.Controllers
{
    public class RouletteController : Controller
    {
        private readonly CasinoContext _context;
        public static int Period;
        public static string Message;
        public static int WinNumber;
        public static string WinColor;

        public RouletteController(CasinoContext context)
        {
            _context = context;
        } 

            
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("LoggedId") == null)
            {
                return RedirectToAction("Login");
            }
            int? userId = (int)HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            Player player = _context.Player.Find((int)userId);
            TempData["ShowWelcome"] = true;
            ViewBag.NickName = player.NickName;
            ViewBag.Period = Period;
            ViewBag.WinColor = WinColor;
            ViewBag.WinNumber = WinNumber;
            if(!string.IsNullOrEmpty(Message))
            {
                ViewBag.Message = Message;
                Message = string.Empty;
            }
            return View();
        }

        public void MakeBet(int Number , string Color , string EvenOdd,int Money)
        {
            if (Number == -1 && string.IsNullOrEmpty(Color) && string.IsNullOrEmpty(EvenOdd)) 
            { 
                Message = "You did not make your bet";
                return;
                //return RedirectToAction("Index");
            }
            int? userId = (int)HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return;
                //return RedirectToAction("Login");
            }
            Bet bet = new Bet
            {
                PlayerId = (int)userId,
                Number = Number,
                Color = Color,
                EvenOdd = EvenOdd,
                Money = Money,
                CreatedAt = DateTime.Now
            };  
            _context.Bet.Add(bet);

            Player player = _context.Player.Find((int)userId);
            player.LastBetAt = DateTime.Now;
            player.IsActive = true;
            _context.SaveChanges();
            //return RedirectToAction("Index");
        }

        public JsonResult ProcessBet(int number, string color, string evenOdd, int money)
        {
            MakeBet(number, color, evenOdd, money);
            List<Bet> bets = _context.Bet.ToList();
            return new JsonResult(bets);
        }

        public JsonResult GetHistory()
        {
            List<History> list = _context.History.OrderByDescending(h=>h.CreatedAt).Take(10).ToList();
            return new JsonResult(list);
        }
    }
}
