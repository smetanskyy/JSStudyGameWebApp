using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerAdditionalInfo> PlayersAdditionalInfo { get; set; }
        public DbSet<PlayerScore> Scores { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}
