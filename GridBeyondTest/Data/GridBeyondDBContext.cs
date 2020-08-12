using GridBeyondTest.Models;
using Microsoft.EntityFrameworkCore;

namespace GridBeyondTest.Data
{
    /// <summary>
    /// Database context
    /// </summary>
    public class GridBeyondDBContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Database options</param>
        public GridBeyondDBContext(DbContextOptions<GridBeyondDBContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Main table
        /// </summary>
        public DbSet<MarketRegisterModel> MarketRegister { get; set; }

        /// <summary>
        /// View with the common statistics
        /// </summary>
        public DbSet<CommonStatisticsModel> CommonStatistics { get; set; }
        
        /// <summary>
        /// We need this method to inform that view has no key
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CommonStatisticsModel>().HasNoKey();
        }


    }
}
