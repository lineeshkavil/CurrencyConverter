using CurrencyConverter.Entities;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.Repository;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Services.DataServices
{
    public class CurrencyServices : ICurrencyService
    {
        private readonly Repository<Currency> _currencyRepository;
        private readonly ILogger<CurrencyServices> _logger;
        public CurrencyServices(Repository<Currency> currencyRepository, ILogger<CurrencyServices> logger)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            _logger.LogInformation("GetAllCurrencies started");
            return await _currencyRepository.GetAll();
        }

        public async Task SaveAllCurrencies(List<Currency> currencies)
        {
            _logger.LogInformation("GetAllCurrencies started");
            foreach (Currency currency in currencies)
            {
                await _currencyRepository.Add(currency);
            }

            await _currencyRepository.SaveChanges();
            _logger.LogInformation("GetAllCurrencies completed");
        }

    }
}
