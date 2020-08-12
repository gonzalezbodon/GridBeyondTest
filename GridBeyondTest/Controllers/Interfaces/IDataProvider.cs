

namespace GridBeyondTest.Controllers.Interfaces
{
    /// <summary>
    /// Interface in charge of loading data from some resource
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Load data from some resource
        /// </summary>
        /// <returns>True on success or false in other case</returns>
        public bool Load();
    }
}
