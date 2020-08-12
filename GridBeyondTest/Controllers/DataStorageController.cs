using GridBeyondTest.Controllers.Interfaces;
using GridBeyondTest.Data;
using GridBeyondTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GridBeyondTest.Controllers
{
    public class DataStorageController : IDataStorage
    {
        private readonly GridBeyondDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private int _maxRegistersToSave;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public DataStorageController(ILogger<DataStorageController> logger, GridBeyondDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;

            //Read the num elements per page
            _maxRegistersToSave = _configuration.GetValue<int>("TableElementsPerPage");
        }

        /// <summary>
        /// Truncate the data table
        /// </summary>
        /// <returns>True on success, false in other case</returns>
        public bool Delete()
        {
            try
            {
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE MarketRegister");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">Number of page to read</param>
        /// <param name="elementsPerPage">Number of elements per page</param>
        /// <param name="order">Field to order (defined in GridBeyondTest.Constants)</param>
        /// <param name="direction">ASC or DESC</param>
        /// <returns>A list of data objects</returns>
        public async Task<List<MarketRegisterModel>> Load(int page, int elementsPerPage, string order = "", string direction="")
        {
            try
            {
                //Query creation, depending on order and direction
                IOrderedQueryable<MarketRegisterModel> query;
                if(order.ToLower() == Constants.ORDER_FIELD_PRICE)
                {
                    query = direction.ToLower() == Constants.ORDER_DESC
                        ? _dbContext.MarketRegister.OrderByDescending(x => x.Price)
                        : _dbContext.MarketRegister.OrderBy(x => x.Price);
                }
                else
                {
                    query = direction.ToLower() == Constants.ORDER_DESC
                        ? _dbContext.MarketRegister.OrderByDescending(x => x.RegistrationDate)
                        : _dbContext.MarketRegister.OrderBy(x => x.RegistrationDate);
                }                    

                //Query execution calculating pagination
                if (page <= 0) page = 1;
                return (await query.ToListAsync())
                    .GetRange((page-1)* elementsPerPage, elementsPerPage);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Saves data in the database per packages
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        public bool Save(List<MarketRegisterModel> registers)
        {
            bool prevDetectChangesConfig = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                
                int registersAnalyzed = 0;
                List<DateTime> currentDatesToSave = new List<DateTime>();
                foreach (MarketRegisterModel register in registers)
                {
                    registersAnalyzed++;

                    //Check if a register with the same datetime exists. (In database there is a UNIQUE CONSTRAINT to be sure)
                    MarketRegisterModel sameDateRegister = _dbContext.MarketRegister.FirstOrDefault(r => r.RegistrationDate == register.RegistrationDate);
                    if (!currentDatesToSave.Contains(register.RegistrationDate) && sameDateRegister == null)
                    {
                        _dbContext.MarketRegister.Add(register);
                        currentDatesToSave.Add(register.RegistrationDate);
                    }
                    else
                    {
                        _logger.LogWarning($"[{DateTime.Now}] Wrong or duplicate data - DATE: {register.RegistrationDate}, PRICE: {register.Price}");
                    }

                    //If we have the maximum number of register to save or it is the last one, we save the pending registers
                    if(currentDatesToSave.Count >= _maxRegistersToSave || (registersAnalyzed == registers.Count && currentDatesToSave.Count > 0))
                    {
                        _dbContext.SaveChanges();
                        currentDatesToSave.Clear();
                    }
                }

                return true;
            }
            catch (Exception){
                return false;
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = prevDetectChangesConfig;
            }
        }
    }
}
