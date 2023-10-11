using CurrencyConverter.Entities;
using CurrencyConverter.Models.ServiceModel;
using CurrencyConverter.Models.ViewModels;
using CurrencyConverter.Repositories;
using CurrencyConverter.Service.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CurrencyConverter.Services.ExternalServices
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private IHttpHelper _httpHelper;
        private readonly ILogger<CurrencyConverterService> _logger;
        public CurrencyConverterService(IHttpHelper httpHelper, ILogger<CurrencyConverterService> logger)
        {
            _httpHelper = httpHelper;
            _logger = logger;
        }
        public async Task<CurrencyRateViewModel> GetCurrencyConvertedRate(string sourceCurrencyCode, string targetCurrencyCode, int amount, DateTime? date =null)
        {

            try
            {
                _logger.LogInformation("GetCurrencyConvertedRate started");
                var apiUrl = string.Empty;
                CurrencyRateViewModel currencyRateViewModel = new();

                if (date.HasValue)
                {
                    apiUrl = string.Format(string.Concat(ApiConstants.CurrencyApiUrl, ApiConstants.SpecificDateRateUrl), date.Value.ToString("yyyy-MM-dd"), sourceCurrencyCode, targetCurrencyCode);
                }
                else
                {
                    apiUrl = string.Format(string.Concat(ApiConstants.CurrencyApiUrl, ApiConstants.LatestRateUrl), sourceCurrencyCode, targetCurrencyCode);
                }

                string responseBody = await _httpHelper.GetHttpRequest(apiUrl);

                var currencyRateModel = JsonConvert.DeserializeObject<CurrencyRateModel>(responseBody);

                if (currencyRateModel.Success)
                {
                    currencyRateViewModel.ExchangeRate = double.Parse(currencyRateModel.ConvertionRates?.Values?.FirstOrDefault()) * amount;
                }
                else if (currencyRateModel.Error != null)
                {
                    string message = string.Empty;

                    switch (currencyRateModel.Error.Code)
                    {
                        case ApiConstants.ErrorCode105:
                            message = ApiConstants.ErrorCode105Message;
                            break;
                        case ApiConstants.ErrorCode201:
                            message = ApiConstants.ErrorCode201Message;
                            break;
                        default:
                            message = ApiConstants.ErrorCodeMessage;
                            break;
                    }

                    currencyRateViewModel.ErrorMessage = message;
                }

                _logger.LogInformation("GetCurrencyConvertedRate completed");
                return currencyRateViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCurrencyConvertedRate error occured", ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task<CurrencyRateModel> GetCurrencyConvertedRatesForMultipleCurrencies(string sourceCurrencyCode, string targetCurrencyCode, int amount, DateTime? date = null)
        {

            try
            {
                _logger.LogInformation("GetCurrencyConvertedRate started");
                var apiUrl = string.Empty;
                CurrencyRateViewModel currencyRateViewModel = new();

                if (date.HasValue)
                {
                    apiUrl = string.Format(string.Concat(ApiConstants.CurrencyApiUrl, ApiConstants.SpecificDateRateUrl), date.Value.ToString("yyyy-MM-dd"), sourceCurrencyCode, targetCurrencyCode);
                }
                else
                {
                    apiUrl = string.Format(string.Concat(ApiConstants.CurrencyApiUrl, ApiConstants.LatestRateUrl), sourceCurrencyCode, targetCurrencyCode);
                }

                string responseBody = await _httpHelper.GetHttpRequest(apiUrl);

               return JsonConvert.DeserializeObject<CurrencyRateModel>(responseBody);

            }
            catch (Exception ex)
            {
                _logger.LogError("GetCurrencyConvertedRatesForMultipleCurrencies error occured", ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Currency>> GetAllCurrencies()
        {

            try
            {
                _logger.LogInformation("GetAllCurrencies started");
                var apiUrl = string.Concat(ApiConstants.CurrencyApiUrl, ApiConstants.CurrencyListUrl);
                CurrencyRateViewModel currencyRateViewModel = new();

                string responseBody = await _httpHelper.GetHttpRequest(apiUrl);

                var currencyModel = JsonConvert.DeserializeObject<CurrencyApiModel>(responseBody);

                if (currencyModel.Success)
                {
                    List<Currency> currencyList = currencyModel.Currencies.Select(kv => new Currency
                    {
                        CurrencyCode = kv.Key,
                        CurrencyName = kv.Value
                    }).ToList();

                    return currencyList;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllCurrencies error occured", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}