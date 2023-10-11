using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Service.Contracts
{
    public interface IHttpHelper
    {
        Task<string> GetHttpRequest(string url);
    }
}
