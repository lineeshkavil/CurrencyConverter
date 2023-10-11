using CurrencyConverter.Repositories.Helpers;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.ExternalServices;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Tests
{
    [TestClass]
    public class TestCurrencyConverterService
    {
        private readonly Mock<ILogger<CurrencyConverterService>> _loggerMock;
        private readonly CurrencyConverterService _currencyRateService;
        private readonly Mock<IHttpHelper> _httpHelper;
        public TestCurrencyConverterService()
        {
            _loggerMock = new Mock<ILogger<CurrencyConverterService>>();
            _httpHelper = new Mock<IHttpHelper>();
            _currencyRateService = new CurrencyConverterService(_httpHelper.Object, _loggerMock.Object);
        }
        [TestMethod]
        public async Task GetCurrencyConvertedRate_SuccessfulRateRetrieval()
        {
            // Arrange
            var sourceCurrencyCode = "EUR";
            var targetCurrencyCode = "USD";
            var amount = 100;
            var date = DateTime.UtcNow;
            var expectedExchangeRate = 0.85; 
            var apiResponse = "{\"success\":true,\"timestamp\":1696876624,\"historical\":true,\"base\":\"EUR\",\"date\":\"2023-10-09\",\"rates\":{\"NOK\":11.406612}}";

            _httpHelper
                .Setup(helper => helper.GetHttpRequest(It.IsAny<string>()))
                .ReturnsAsync(apiResponse);


            var result = await _currencyRateService.GetCurrencyConvertedRate(sourceCurrencyCode, targetCurrencyCode, amount, date);

       
            Assert.IsNotNull(result.ExchangeRate);
           
        }

        [TestMethod]
        public async Task GetCurrencyConvertedRate_ApiErrorHandling()
        {
            // Arrange
            var sourceCurrencyCode = "USD";
            var targetCurrencyCode = "EUR";
            var amount = 100;
            var date = DateTime.UtcNow;
            var errorCode = "105";
            var apiResponse = $"{{ \"success\": false, \"error\": {{ \"code\": \"{errorCode}\" }} }}";
            var expectedMessage = "The current subscription plan does not support this Currency. Please select any other currency";
            _httpHelper
                .Setup(helper => helper.GetHttpRequest(It.IsAny<string>()))
                .ReturnsAsync(apiResponse);
            
            var result = await _currencyRateService.GetCurrencyConvertedRate(sourceCurrencyCode, targetCurrencyCode, amount, date);

           
            Assert.IsNotNull(result.ErrorMessage);

        }
    }
}
