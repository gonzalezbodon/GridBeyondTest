using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GridBeyondTest.Controllers.Interfaces;
using GridBeyondTest.Models;
using GridBeyondTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GridBeyondTest.Controllers
{
    /// <summary>
    /// Class used by ajax calls, to save and delete data and consulting statistics
    /// It is an example of a REST api page
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MarketRegisterController : ControllerBase
    {
        private readonly IAnalyzer _analyzer;
        private readonly IDataStorage _dataStorage;
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="dataStorage"></param>
        /// <param name="dataProvider"></param>
        public MarketRegisterController(IAnalyzer analyzer, IDataStorage dataStorage, IDataProvider dataProvider)
        {
            _analyzer = analyzer;
            _dataStorage = dataStorage;
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Offers the common statistics from the data
        /// </summary>
        /// <returns>A json object with the common statistics</returns>
        [HttpGet("statistics")]
        public async Task<CommonStatisticsModel> GetCommonStatistics()
        {
            try
            {
                return await _analyzer.GetCommonStatistics();
            }
            catch (Exception)
            {
                return new CommonStatisticsModel
                {
                    TotalItems = 0
                };
            }
        }

        /// <summary>
        /// Load all data
        /// </summary>
        /// <returns>A simple response to inform about the result</returns>
        [HttpPost]
        public SimpleResponseViewModel LoadData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool success = _dataProvider.Load();
            sw.Stop();
            return success 
                ? new SimpleResponseViewModel
                {
                    code = 0,
                    message = $"Data loaded succesfully in {sw.ElapsedMilliseconds}ms"
                }
                : new SimpleResponseViewModel
                {
                    code = 1,
                    message = "Error loading data, try again later"
                };
           
        }

        /// <summary>
        /// Delete all data
        /// </summary>
        /// <returns>A simple response to inform about the result</returns>
        [HttpDelete]
        public SimpleResponseViewModel DeleteData()
        {
            return _dataStorage.Delete()
                ? new SimpleResponseViewModel
                {
                    code = 0,
                    message = "Data deleted succesfully"
                }
                : new SimpleResponseViewModel
                {
                    code = 1,
                    message = "Error deleting data, try again later"
                };
        }
    }
}
