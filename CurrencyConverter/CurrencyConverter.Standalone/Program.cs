using CurrencyConverter.Entities;
using CurrencyConverter.Models.ViewModels;
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
            .SetBasePath(Directory.GetCurrentDirectory())
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
//factory.AddNLog();
//factory.ConfigureNLog("nlog.config");

var logger = LogManager.GetCurrentClassLogger();
try
{
    LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
    logger.Info("Currency converter started.");
    var currencies = await currencyRepository.GetAllCurrencies();

    Console.WriteLine("Currency Converter");

    Console.Write("Enter the source currency code: ");
    string sourceCurrencyCode = Console.ReadLine().ToUpper();



    if (!currencies.Any(x => x.CurrencyCode == sourceCurrencyCode))
    {
        Console.WriteLine("Invalid source currency code.");
        return;
    }

    Console.Write("Enter the target currency code: ");
    string targetCurrencyCode = Console.ReadLine().ToUpper();

    if (!currencies.Any(x => x.CurrencyCode == targetCurrencyCode))
    {
        Console.WriteLine("Invalid target currency code.");
        return;
    }

    Console.Write("Enter the amount in {0}: ", sourceCurrencyCode);
    if (!int.TryParse(Console.ReadLine(), out int amount))
    {
        Console.WriteLine("Invalid amount.");
        return;
    }

    var exchangeValue = await ConvertCurrency(amount, sourceCurrencyCode, targetCurrencyCode);
    var convertedAmount = string.Empty;

    if (exchangeValue != null && string.IsNullOrEmpty(exchangeValue.ErrorMessage))
    {
        convertedAmount = exchangeValue.ExchangeRate.ToString();
        var baseCurrencyName = currencies.FirstOrDefault(x => x.CurrencyCode == sourceCurrencyCode).CurrencyName;
        var targetCurrencyName = currencies.FirstOrDefault(x => x.CurrencyCode == targetCurrencyCode).CurrencyName;
        Console.WriteLine("{0} {1} is equivalent to {2} {3}", amount, baseCurrencyName, convertedAmount, targetCurrencyName);
    }
    else
    {
        convertedAmount = exchangeValue.ErrorMessage;
        Console.WriteLine(convertedAmount);
    }
    logger.Info("Currency converter end.");
}
catch (Exception ex)
{
    logger.Error("An error occurred: {0}", ex.Message);
}


 async Task<CurrencyRateViewModel> ConvertCurrency(int amount, string sourceCurrency, string targetCurrency)
{
    var currencyExchangeRepository = serviceProvider.GetService<ICurrencyConverterService>();
    var exchangeRates = await currencyExchangeRepository.GetCurrencyConvertedRate(sourceCurrency, targetCurrency, amount);
   

    return exchangeRates;
}



