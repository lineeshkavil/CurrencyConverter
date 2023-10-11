using CurrencyConverter.Entities;
using CurrencyConverter.Models.ViewModels;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Services.DataServices
{
    public class CurrencyExchangeRateService : ICurrencyExchangeRateService
    {
        private readonly Repository<CurrencyExchangeRate> _currencyExchangeRate;
        private readonly CurrencyDbContext _context;
        public CurrencyExchangeRateService(Repository<CurrencyExchangeRate> currencyExchangeRate, CurrencyDbContext context)
        {
            _currencyExchangeRate = currencyExchangeRate;
            _context = context;
        }

        public async Task<CurrencyCompareModel> GetAllCurrencyExchangeRates(string baseCurrency, string targetCurrency, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                IQueryable<CurrencyExchangeRate> exchangeRate = _context.CurrencyExchangeRates.Include(x => x.SourceCurrency).Include(x => x.TargetCurrency)
                        .Where(x => x.SourceCurrency.CurrencyCode == baseCurrency && x.TargetCurrency.CurrencyCode == targetCurrency);

                if (startDate.HasValue)
                {
                    exchangeRate = exchangeRate.Where(x => x.Date >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    exchangeRate = exchangeRate.Where(x => x.Date <= endDate.Value);
                }

                var rates = await exchangeRate.ToListAsync();


                string exchangeRates = string.Join(", ", rates.Select(p => Math.Round(p.ExchangeRate, 3)));
                string exchangeDates = string.Join(", ", rates.Select(p => p.Date.ToShortDateString()));
                var model = new CurrencyCompareModel()
                {
                    FromCurrency = baseCurrency,
                    ToCurrency = targetCurrency,
                    Dates = exchangeDates,
                    Rates = exchangeRates
                };

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CurrencyExchangeRate>> GetAllCurrencyExchangeRatesBetween()
        {

            try
            {
                return await _context.CurrencyExchangeRates.Include(x => x.SourceCurrency).Include(x => x.TargetCurrency).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SaveExchangeRates(List<CurrencyExchangeRate> currencyExchangeRates)
        {
            try
            {
                foreach (var currencyExchangeRate in currencyExchangeRates)
                {
                    await _currencyExchangeRate.Add(currencyExchangeRate);
                }

                await _currencyExchangeRate.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveExchangeRate(CurrencyExchangeRate currencyExchangeRate)
        {
            try
            {
                await _currencyExchangeRate.Add(currencyExchangeRate);
                await _currencyExchangeRate.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CurrencyExchangeRate> GetlastInsertedRecordByDate(DateTime date)
        {
            try
            {
                return await _context.CurrencyExchangeRates.FirstOrDefaultAsync(x => x.Date.Date == date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
