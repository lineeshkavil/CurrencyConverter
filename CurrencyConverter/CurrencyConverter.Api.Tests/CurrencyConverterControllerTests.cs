using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using Xunit;

using CurrencyConverter.Entities;

using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Api.Controllers;
using CurrencyConverter.Services.DataServices;
using CurrencyConverter.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Http;

namespace CurrencyConverter.Api.Tests
{
    [TestClass]
    public class CurrencyConverterControllerTests
    {
        private Mock<ICurrencyConverterService> _currencyConverterRepository;
        private Mock<ICurrencyExchangeRateService> _currencyExchangeRateRepository;
        private Mock<CurrencyDbContext> _context;
        private Mock<ICurrencyService> _currencyRepository;
        private Mock<IUserService> _userRepository;
        private readonly CurrencyConverterController _currencyConverterController;
        private readonly Mock<ILogger<CurrencyConverterController>> _logger;

        public CurrencyConverterControllerTests()
        {
            _currencyConverterRepository = new Mock<ICurrencyConverterService>();
            _logger = new Mock<ILogger<CurrencyConverterController>>();
            var options = new DbContextOptionsBuilder<CurrencyDbContext>()
      .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
      .Options;
            using var dbContext = new CurrencyDbContext(options);

            _currencyExchangeRateRepository = new Mock<ICurrencyExchangeRateService>();
            _currencyRepository = new Mock<ICurrencyService>();
            _userRepository = new Mock<IUserService>();

            _currencyConverterController = new CurrencyConverterController(_currencyConverterRepository.Object, dbContext, _currencyRepository.Object
               , _currencyExchangeRateRepository.Object, _logger.Object, _userRepository.Object);
        }
        [TestMethod]
        public async Task GetCurrencyConvertedRate_ReturnsOkResult_WithValidData()
        {
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100;
            var date = DateTime.Now;

            var expectedExchangeRates = new CurrencyRateViewModel
            {
                ExchangeRate = 0.85,

            };

            _currencyConverterRepository.Setup(repo =>
                repo.GetCurrencyConvertedRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(expectedExchangeRates);

            var result = await _currencyConverterController.GetCurrencyConverteRate(baseCurrency, targetCurrency, amount, date);


            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetCurrencyConvertedRate_ReturnsNull_WhenRepositoryReturnsNull()
        {
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100;
            var date = DateTime.Now;
            var currencyRateViewModel = new CurrencyRateViewModel();
            _currencyConverterRepository.Setup(repo =>
        repo.GetCurrencyConvertedRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime?>()))
        .Returns(Task.FromResult<CurrencyRateViewModel>(null));
            var result = await _currencyConverterController.GetCurrencyConverteRate(baseCurrency, targetCurrency, amount, date);

            Assert.AreEqual(result, null);
        }
        [TestMethod]
        public async Task GetAvailableCurrencies_ReturnsOkResult_WithValidData()
        {

            var expectedCurrencies = new List<Currency>
            {
                new Currency { CurrencyId = 1, CurrencyCode = "AED" },
                new Currency { CurrencyId = 2, CurrencyCode = "AFN" },

            };

            _currencyRepository.Setup(repo => repo.GetAllCurrencies())
                .ReturnsAsync(expectedCurrencies);

            var result = await _currencyConverterController.GetAvailableCurrencies();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAvailableCurrencies_ThrowsException_WhenRepositoryFails()
        {
            var expectedExceptionMessage = "Repository error message";


            _currencyRepository.Setup(repo => repo.GetAllCurrencies())
                .ThrowsAsync(new Exception(expectedExceptionMessage));

          var exception =  await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await _currencyConverterController.GetAvailableCurrencies();
            });

            Assert.AreEqual(expectedExceptionMessage, exception.Message);

        }

        [TestMethod]
        public async Task GetCurrencyExchangeRates_ReturnsOkResult_WithValidData()
        {
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var startDate = DateTime.Now.AddDays(-2);
            var endDate = DateTime.Now;

            var expectedExchangeRates = new CurrencyCompareModel
            {
                Rates ="",
                FromCurrency = baseCurrency,
                ToCurrency = targetCurrency

            };

            _currencyExchangeRateRepository.Setup(repo =>
                repo.GetAllCurrencyExchangeRates(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(expectedExchangeRates);

            var result = await _currencyConverterController.GetCurrencyExchangeRates(baseCurrency, targetCurrency, startDate, endDate);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetCurrencyExchangeRates_ReturnsNull_WhenRepositoryReturnsNull()
        {
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100;
            var date = DateTime.Now;
            var currencyRateViewModel = new CurrencyRateViewModel();
            _currencyExchangeRateRepository.Setup(repo =>
        repo.GetAllCurrencyExchangeRates(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
        .Returns(Task.FromResult<CurrencyCompareModel>(null));
            var result = await _currencyConverterController.GetCurrencyConverteRate(baseCurrency, targetCurrency, amount, date);

            Assert.AreEqual(result, null);
        }
    }
}