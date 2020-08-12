using GridBeyondTest.Controllers.Interfaces;
using GridBeyondTest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GridBeyondTest.Controllers
{
    /// <summary>
    /// Class that implements IDataProvider interface
    /// </summary>
    public class DataProviderController : IDataProvider
    {
        private readonly ILogger<DataProviderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDataStorage _dataStorage;
        
        private int _maxRegistersToRead;

        string _sourceFile;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="dataStorage"></param>
        public DataProviderController(ILogger<DataProviderController> logger, IConfiguration configuration, IDataStorage dataStorage)
        {
            _logger = logger;
            _configuration = configuration;
            _dataStorage = dataStorage;

            _sourceFile = _configuration.GetValue<string>("SourceFile");
            _maxRegistersToRead = _configuration.GetValue<int>("MaxRegisterToRead");
           
        }

        /// <summary>
        /// Load Data from the file offered by the configuration object
        /// </summary>
        /// <returns>True on success, false in other case</returns>
        public bool Load()
        {
            try
            {
                
                int linesReaded = 0;
                List<MarketRegisterModel> registers;
                
                do
                {
                    //Read a set of data
                    linesReaded = ReadRegistersFromFile(out registers, linesReaded, _maxRegistersToRead);
                    //Save data if exists
                    if (registers.Count > 0) _dataStorage.Save(registers);

                }
                while (registers.Count > 0);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Read a subset of data from the file offered by the configuration object
        /// </summary>
        /// <param name="registers">Object where the method returns the result</param>
        /// <param name="startLine">Line number from which to read</param>
        /// <param name="maxLines">Lines number to read</param>
        /// <returns>Number of lines readed</returns>

        private int ReadRegistersFromFile(out List<MarketRegisterModel> registers, int startLine = 0, int maxLines = 0)
        {
            registers = new List<MarketRegisterModel>();

            int lineNumber = 0;

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            using (StreamReader reader = new StreamReader(_sourceFile))
            {
                while (reader.Peek() >= 0 && (maxLines == 0 || registers.Count < maxLines))
                {
                    //Read line
                    string line = reader.ReadLine();
                    lineNumber++;

                    if (lineNumber <= startLine) continue;

                    //Attempt to format data and save it in the list
                    try
                    {
                        List<string> fields = line.Split(",").Select(x => x.Trim()).ToList();
                        registers.Add(new MarketRegisterModel
                        {
                            RegistrationDate = DateTime.ParseExact(fields[0], "dd/MM/yyyy HH:mm", null),
                            Price = Convert.ToDouble(fields[1], provider)
                        });
                    }
                    catch (Exception)
                    {
                        _logger.LogWarning($"[{DateTime.Now}] Incorrect format - Line {lineNumber}. {line}");
                        continue;
                    }
                }
            }

            return lineNumber;
        }
    }
}
