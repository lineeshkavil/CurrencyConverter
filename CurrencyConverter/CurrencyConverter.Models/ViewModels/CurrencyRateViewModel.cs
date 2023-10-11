using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.ViewModels
{
    public class CurrencyRateViewModel
    {
        public double ExchangeRate { get; set; }
        public string ErrorMessage { get; set; }
    }
}
