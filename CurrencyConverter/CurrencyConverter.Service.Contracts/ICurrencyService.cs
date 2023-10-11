using CurrencyConverter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Service.Contracts
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllCurrencies();
        Task SaveAllCurrencies(List<Currency> currencies);
    }
}
