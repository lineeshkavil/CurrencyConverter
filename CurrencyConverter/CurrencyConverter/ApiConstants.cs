using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Repositories
{
    public class ApiConstants
    {
        public const string CurrencyApiUrl = "http://data.fixer.io/api";
        public const string LatestRateUrl = "/latest?access_key=af4576d63000a9c73dd2779c03a5783f&base={0}&symbols={1}&format=1";
        public const string SpecificDateRateUrl = "/{0}?access_key=af4576d63000a9c73dd2779c03a5783f&base={1}&symbols={2}&format=1";
        public const string CurrencyListUrl = "/symbols?access_key=af4576d63000a9c73dd2779c03a5783f&format=1";


        public const int ErrorCode105 = 105;
        public const string ErrorCode105Message = "The current subscription plan does not support this Currency. Please select any other currency";
        public const int ErrorCode201 = 201;
        public const string ErrorCode201Message = "An invalid base currency has been entered. Please select  valid currency";

        public const string ErrorCodeMessage = "An error occured!. Please try after sometime";



    }
}
