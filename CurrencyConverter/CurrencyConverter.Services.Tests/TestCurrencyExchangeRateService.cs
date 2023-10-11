using CurrencyConverter.Entities;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.DataServices;
using CurrencyConverter.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CurrencyConverter.Services.Tests
{
    [TestClass]
    public class TestCurrencyExchangeRateService
    {
        private Mock<Repository<CurrencyExchangeRate>> _currencyExchangeRate;
        private Mock<CurrencyDbContext> _context;
        private readonly CurrencyExchangeRateService _exchangeService;

        public TestCurrencyExchangeRateService()
        {
            var options = new DbContextOptionsBuilder<CurrencyDbContext>()
    .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
    .Options;
             var dbContext = new CurrencyDbContext(options);
             _currencyExchangeRate = new Mock<Repository<CurrencyExchangeRate>>(dbContext);


            _exchangeService = new CurrencyExchangeRateService(_currencyExchangeRate.Object, dbContext);
        }
        [TestMethod]
        public async Task GetAllCurrencyExchangeRates_ValidInput_NoDateRange()
        {
            // Arrange
            string baseCurrency = "USD";
            string targetCurrency = "EUR";

            // Act
            var result = await _exchangeService.GetAllCurrencyExchangeRates(baseCurrency, targetCurrency);

            // Assert
            Assert.IsNotNull(result);
            
        }
        [TestMethod]
        public async Task GetAllCurrencyExchangeRates_ValidInput_WithDateRange()
        {
            // Arrange
          
            string baseCurrency = "USD";
            string targetCurrency = "EUR";
            DateTime startDate = DateTime.Parse("2023-01-01");
            DateTime endDate = DateTime.Parse("2023-12-31");

            // Act
            var result = await _exchangeService.GetAllCurrencyExchangeRates(baseCurrency, targetCurrency, startDate, endDate);

            // Assert
            Assert.IsNotNull(result);
         
        }


        [TestMethod]
        public async Task GetAllCurrencyExchangeRates_InvalidTargetCurrency()
        {
           
            // Arrange
            
            string baseCurrency = "USD";
            string targetCurrency = "XYZ"; // An invalid currency code

            // Act & Assert
            var result = await  _exchangeService.GetAllCurrencyExchangeRates(baseCurrency, targetCurrency);
            Assert.AreEqual("", result.Dates);
        }
        [TestMethod]
        public async Task GetAllCurrencyExchangeRates_InvalidDateRange()
        {
            // Arrange
          
            string baseCurrency = "USD";
            string targetCurrency = "EUR";
            DateTime startDate = DateTime.Parse("2023-12-31");
            DateTime endDate = DateTime.Parse("2023-01-01"); // Invalid date range

            // Act & Assert
            var result = await   _exchangeService.GetAllCurrencyExchangeRates(baseCurrency, targetCurrency, startDate, endDate);
            Assert.AreEqual("", result.Dates);
        }
    }
}