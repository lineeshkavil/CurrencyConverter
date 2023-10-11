using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.ViewModels
{
    public class CurrencyCompareModel
    {
        public string Rates { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public string Dates { get; set; }
    }
}
