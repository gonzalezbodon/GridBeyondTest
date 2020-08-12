using GridBeyondTest.Models;
using System.Threading.Tasks;

namespace GridBeyondTest.Controllers.Interfaces
{
    /// <summary>
    /// Interface in charge of data analyzing
    /// </summary>
    public interface IAnalyzer
    {
        /// <summary>
        /// Method for calcule the main statistics
        /// </summary>
        /// <returns>Object CommonStatisticsModel with main statistics data</returns>
        public Task<CommonStatisticsModel> GetCommonStatistics();
    }
}
