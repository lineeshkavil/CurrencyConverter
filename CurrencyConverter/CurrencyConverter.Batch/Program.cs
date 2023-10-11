using CurrencyConverter.Entities;
using CurrencyConverter.Repositories.Helpers;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.DataServices;
using CurrencyConverter.Services.ExternalServices;
using CurrencyConverter.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

var serviceProvider = new ServiceCollection()
        .AddDbContext<CurrencyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        })

            .AddSingleton<IHttpHelper, HttpHelper>()
.AddSingleton<ICurrencyService, CurrencyServices>()
.AddSingleton<Repository<Currency>>()
.AddSingleton<Repository<CurrencyExchangeRate>>()
.AddSingleton<ICurrencyConverterService, CurrencyConverterService>()
.AddSingleton<ICurrencyExchangeRateService, CurrencyExchangeRateService>()
.AddLogging()
        .BuildServiceProvider();
var currencyRepository = serviceProvider.GetService<ICurrencyService>();
var factory = serviceProvider.GetService<ILoggerFactory>();
var logger = LogManager.GetCurrentClassLogger();

try
{
    LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
    logger.Info("Currency converter batch started.");

    var convertionDate = DateTime.Now.Date;

    var currencyExchangeRepository = serviceProvider.GetService<ICurrencyExchangeRateService>();
    var lastInsertedRecord = await currencyExchangeRepository.GetlastInsertedRecordByDate(convertionDate);

    if (lastInsertedRecord == null)
    {
        var baseCurrencyCode = "EUR";
        var minimumAmount = 1;

        var currencies = await currencyRepository.GetAllCurrencies();
        var baseCurrency = currencies.FirstOrDefault(x => x.CurrencyCode == baseCurrencyCode);
        var baseCurrencyId = baseCurrency.CurrencyId;
        currencies.Remove(baseCurrency);
        var countryCodes = string.Join(", ", currencies.Select(x => x.CurrencyCode));


        var currencyConvertRepository = serviceProvider.GetService<ICurrencyConverterService>();
        var exchangeRates = await currencyConvertRepository.GetCurrencyConvertedRatesForMultipleCurrencies(baseCurrencyCode, countryCodes, minimumAmount, convertionDate);


        List<CurrencyExchangeRate> currencyExchangeRates = exchangeRates.ConvertionRates.Select(kv => new CurrencyExchangeRate
        {
            SourceCurrencyId = baseCurrencyId,
            TargetCurrencyId = currencies.FirstOrDefault(x => x.CurrencyCode == kv.Key).CurrencyId,
            ExchangeRate = Convert.ToDecimal(double.Parse(kv.Value)),
            Date = convertionDate
        }).ToList();

        await currencyExchangeRepository.SaveExchangeRates(currencyExchangeRates);
    }
    else
    {
        logger.Info("Currency converter batch already executed today.");
    }

    logger.Info("Currency converter batch completed.");
    // Console.ReadLine();
}
catch (Exception ex)
{

    logger.Error("Currency converter error occured", ex.Message);
    throw ex;
}



