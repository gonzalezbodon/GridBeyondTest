using GridBeyondTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GridBeyondTest.Controllers.Interfaces
{
    /// <summary>
    /// Interface in charge of managing the data storage
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// Stores the data receives by parameter
        /// </summary>
        /// <param name="registers">List of registers for save</param>
        /// <returns>True on success or false in other case</returns>
        public bool Save(List<MarketRegisterModel> registers);

        /// <summary>
        /// Returns a subset of data
        /// </summary>
        /// <param name="page">Page number to read</param>
        /// <param name="elementsPerPage">Elements number per page</param>
        /// <param name="order">Field by which we want to sort</param>
        /// <param name="direction">Ascendant or descendant</param>
        /// <returns>List with registers</returns>
        public Task<List<MarketRegisterModel>> Load(int page, int elementsPerPage, string order = "", string direction = "");

        /// <summary>
        /// Deletes all data
        /// </summary>
        /// <returnsTrue on success or false in other case></returns>
        public bool Delete();
    }
}
