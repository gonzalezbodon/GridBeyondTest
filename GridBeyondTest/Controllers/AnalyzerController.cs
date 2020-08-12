using GridBeyondTest.Controllers.Interfaces;
using GridBeyondTest.Data;
using GridBeyondTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GridBeyondTest.Controllers
{
    /// <summary>
    /// Class that implements IAnalyzer interface
    /// </summary>
    public class AnalyzerController : IAnalyzer
    {
        GridBeyondDBContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        public AnalyzerController(GridBeyondDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Calculates common statistics
        /// </summary>
        /// <returns>Object with the main statistics data</returns>
        public async Task<CommonStatisticsModel> GetCommonStatistics()
        {
            //Data offered by a view on database, in this way, we not need to read all data
            CommonStatisticsModel statistics = await _dbContext.CommonStatistics.FirstAsync();
            statistics.TotalItems = await _dbContext.MarketRegister.CountAsync();
            return statistics;
        }
    }
}
