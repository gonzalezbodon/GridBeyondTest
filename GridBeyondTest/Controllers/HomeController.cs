using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GridBeyondTest.Models;
using GridBeyondTest.Controllers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GridBeyondTest.Controllers
{
    /// <summary>
    /// Controller to manage the web pages
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IDataStorage _dataStorage;
        private readonly IConfiguration _configuration;

        int numElementsPerPage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataStorage">To read data</param>
        /// <param name="configuration">To obtain some configuration data</param>
        public HomeController(IDataStorage dataStorage, IConfiguration configuration)
        {
            _dataStorage = dataStorage;
            _configuration = configuration;
            numElementsPerPage = _configuration.GetValue<int>("TableElementsPerPage");
        }

        /// <summary>
        /// Load the only web page
        /// </summary>
        /// <param name="page">Number of page of data to show</param>
        /// <param name="order">Order of the data</param>
        /// <param name="direction">Direction of the data</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page, string order, string direction)
        {
            //Check parameters received by the user
            if (order == null || !new List<string>() { Constants.ORDER_FIELD_DATE, Constants.ORDER_FIELD_PRICE }.Contains(order.ToLower())) 
                order = Constants.ORDER_FIELD_DATE;
            if (direction == null || !new List<string>() { Constants.ORDER_DESC, Constants.ORDER_ASC }.Contains(direction.ToLower())) 
                direction = Constants.ORDER_ASC;

            //Read results
            List<MarketRegisterModel> results;
            try
            {
                results = (await _dataStorage.Load(page, numElementsPerPage, order, direction));
            }
            catch (Exception) { results = null; }

            if(results == null) results = new List<MarketRegisterModel>();

            //Extra data
            ViewData["page"] = page < 1 ? 1 : page;
            ViewData["nextPage"] = page < 1 ? 2 : page + 1;
            ViewData["prevPage"] = page > 1 ? page -1 : page;
            ViewData["order"] = order;
            ViewData["orderPrice"] = Constants.ORDER_FIELD_PRICE;
            ViewData["orderDate"] = Constants.ORDER_FIELD_DATE;
            ViewData["orderDirectionDate"] = order == Constants.ORDER_FIELD_DATE && direction == Constants.ORDER_ASC ? Constants.ORDER_DESC : Constants.ORDER_ASC;
            ViewData["orderDirectionPrice"] = order == Constants.ORDER_FIELD_PRICE && direction == Constants.ORDER_ASC ? Constants.ORDER_DESC : Constants.ORDER_ASC;

            //Return view
            return View(results);
        }
    }
}
