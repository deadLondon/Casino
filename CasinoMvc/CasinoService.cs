
using CasinoMvc.Controllers;
using CasinoMvc.Data;
using CasinoMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace CasinoMvc
{
    public class CasinoService : BackgroundService
    {
        private readonly IServiceScopeFactory _service;

        public CasinoService(IServiceScopeFactory service)
        {
            _service = service;
        }

        static int Period = 60;
        static int WinNumber;
        static string WinColor;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WinNumber = new Random().Next(0, 37);
            WinColor = WinNumber == 0 ? "green" : (new Random().Next(0, 100) % 2 == 0 ? "black" : "red");
            while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _service.CreateScope();
                    using var _context = scope.ServiceProvider.GetRequiredService<CasinoContext>();

                    Period--;

                    if (Period < 0)
                    {
                        var inactiveThreshold = DateTime.Now.AddMinutes(-5);
                        var toDeactivate = await _context.Player
                        .Where(p => p.IsActive && p.LastBetAt < inactiveThreshold)
                        .ToListAsync(stoppingToken);

                        foreach (var player in toDeactivate)
                        {
                            player.IsActive = false;
                        }

                        if (toDeactivate.Any())
                            await _context.SaveChangesAsync(stoppingToken);

                        WinNumber = new Random().Next(0, 37);
                        WinColor = new Random().Next(0, 100) % 2 == 0 ? "black" : "red";
                        if (WinNumber == 0) WinColor = "green";

                        await ProcessBets(_context);

                        Period = 60;
                    }

                    RouletteController.Period = Period;
                    RouletteController.WinNumber = WinNumber;
                    RouletteController.WinColor = WinColor;

                    await Task.Delay(1000, stoppingToken);
                }
            }


        private async Task ProcessBets(CasinoContext context)
        {
            List<Bet> bets = context.Bet.ToList();
            foreach (Bet item in bets)
            {
                int factor = 0;
                if (item.Number == WinNumber)
                {
                    factor += 35;

                }
                if(item.Color == WinColor)
                {
                    factor += 1;

                }
                if(item.EvenOdd == "even" && WinNumber %2 == 0)
                {
                    factor += 1;

                }
                if(item.EvenOdd == "odd" && WinNumber %2 != 0)
                {
                    factor += 1;

                }
                if (factor>0) 
                {
                    factor += 1;
                }
                else  
                {
                    factor -= 1;
                }
                Player player = context.Player.Find(item.PlayerId);
                if (player != null) 
                {
                    player.Balance += factor * item.Money;
                    player.TotalBets ++;
                    if(factor > 0) player.TotalWins ++;
                }
                PlayerArchive pa = new PlayerArchive{
                    PlayerId = item.PlayerId,
                    CreatedAt = DateTime.Now,
                    BetInfo = string.Format("Number:{0}, Color:{1},EvenOdd:{2}-Money:{3}", WinNumber, WinColor, item.EvenOdd,item.Money),
                    Result = factor * item.Money,
                    Money = item.Money
                };
                context.PlayerArchive.Add(pa);
            }
            History history = new History()
            {
                Number = WinNumber,
                Color = WinColor,
                CreatedAt = DateTime.Now
            };
            context.History.Add(history);
            context.Bet.RemoveRange(bets);
            await context.SaveChangesAsync();
        }
    }
}
