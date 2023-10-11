using CurrencyConverter.Entities;
using CurrencyConverter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Service.Contracts
{
    public interface ICurrencyExchangeRateService
    {
        Task<CurrencyCompareModel> GetAllCurrencyExchangeRates(string baseCurrency, string targetCurrency, DateTime? startDate = null, DateTime? endDate = null);
        Task SaveExchangeRate(CurrencyExchangeRate currencyExchangeRate);
        Task SaveExchangeRates(List<CurrencyExchangeRate> currencyExchangeRates);
        Task<CurrencyExchangeRate> GetlastInsertedRecordByDate(DateTime date);
    }
}
