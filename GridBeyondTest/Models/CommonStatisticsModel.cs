using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridBeyondTest.Models
{
    /// <summary>
    /// Model to store the statistics
    /// </summary>
    public class CommonStatisticsModel
    {
        /// <summary>
        /// Example of data annotation
        /// </summary>
        [NotMapped]
        public int TotalItems { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double AvgPrice { get; set; }
        public DateTime MostExpensiveHourDate { get; set; }
        
        /// <summary>
        /// Example of calculated attribute
        /// </summary>
        public DateTime MostExpensiveHourDateEnd => MostExpensiveHourDate.AddHours(1);
        public double MostExpensiveHourPrice { get; set; }
    }
}
