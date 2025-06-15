using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CasinoMvc.Models;

namespace CasinoMvc.Data
{
    public class CasinoContext : DbContext
    {
        public CasinoContext (DbContextOptions<CasinoContext> options)
            : base(options)
        {
        }

        public DbSet<CasinoMvc.Models.Player> Player { get; set; } = default!;
        public DbSet<CasinoMvc.Models.PlayerArchive> PlayerArchive { get; set; } = default!;
        public DbSet<CasinoMvc.Models.Bet> Bet { get; set; } = default!;
        public DbSet<CasinoMvc.Models.History> History { get; set; } = default!;
    }
}
