using System;
using System.ComponentModel;

namespace GridBeyondTest.Models
{
    /// <summary>
    /// Main object model of the program, it is the model for registers
    /// In this model there are examples of attribute annotations, in this case to override the name to show 
    /// </summary>
    public class MarketRegisterModel
    {

        public int Id { get; set; }
        [DisplayName("Register Date")]
        public DateTime RegistrationDate { get; set; }
        [DisplayName("Price (€)")]
        public double Price { get; set; }
    }
}
