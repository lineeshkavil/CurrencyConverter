using CurrencyConverter.Entities;
using CurrencyConverter.Models.ServiceModel;
using CurrencyConverter.Models.ViewModels;

namespace CurrencyConverter.Service.Contracts
{
    public interface ICurrencyConverterService
    {
        Task<CurrencyRateViewModel> GetCurrencyConvertedRate(string sourceCurrencyCode, string targetCurrencyCode, int amount, DateTime? date = null);
        Task<List<Currency>> GetAllCurrencies();
        Task<CurrencyRateModel> GetCurrencyConvertedRatesForMultipleCurrencies(string sourceCurrencyCode, string targetCurrencyCode, int amount, DateTime? date = null);
    }
}