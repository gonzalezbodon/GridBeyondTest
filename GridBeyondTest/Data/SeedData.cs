using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GridBeyondTest.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Class used at to seed data in database to test
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new GridBeyondDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<GridBeyondDBContext>>()))
            {
                /*if (context.MarketRegisters.Any())
                {
                    return;   // DB has been seeded
                }

                context.MarketRegisters.AddRange(
                    new MarketRegister
                    {
                        Price = 20
                    },

                    new MarketRegister
                    {
                        Price = 30
                    },

                    new MarketRegister
                    {
                        Price = 40
                    }
                );
                context.SaveChanges();*/
            }
        }
    }
}
