using CurrencyConverter.Entities;
using CurrencyConverter.Models;
using CurrencyConverter.Models.ViewModels;
using CurrencyConverter.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CurrencyConverter.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private ICurrencyConverterService _currencyConverterRepository;
        private ICurrencyExchangeRateService _currencyExchangeRateRepository;
        private CurrencyDbContext _context;
        private ICurrencyService _currencyRepository;
        private IUserService _userRepository;
        private readonly ILogger<CurrencyConverterController> _logger;
        public CurrencyConverterController(ICurrencyConverterService currencyConverterRepository, CurrencyDbContext context,
            ICurrencyService currencyRepository, ICurrencyExchangeRateService currencyExchangeRateRepository,
            ILogger<CurrencyConverterController> logger, IUserService userRepository)
        {
            _currencyConverterRepository = currencyConverterRepository;
            _context = context;
            _currencyRepository = currencyRepository;
            _currencyExchangeRateRepository = currencyExchangeRateRepository;
            _userRepository = userRepository;
            _logger = logger;
        }


        [HttpGet]
        [Route("Authorize")]
        public async Task<ActionResult<string>> Authorize(string userName, string password)
        {
            try
            {
                _logger.LogInformation("Authorize started");
                var isValidUser = await _userRepository.GetUser(userName, password);

                if (isValidUser)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,userName),
                        new Claim(ClaimTypes.NameIdentifier,password)
                    };

                    var Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt");
                    var token = new JwtSecurityToken
                    (
                        issuer: Config["Issuer"],
                        audience: Config["Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(60),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Key"])),
                            SecurityAlgorithms.HmacSha256)
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenString);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Authorize failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetExchangeRate/{baseCurrency}/{targetCurrency}/{amount}/{date}")]
        public async Task<ActionResult<CurrencyRateViewModel>> GetCurrencyConverteRate(string baseCurrency, string targetCurrency, int amount, DateTime? date = null)
        {
            try
            {
                _logger.LogInformation("GetCurrencyConverteRate started");
                var exchangeRates = await _currencyConverterRepository.GetCurrencyConvertedRate(baseCurrency, targetCurrency, amount, date);

                if (exchangeRates == null)
                {
                    _logger.LogInformation("GetCurrencyConverteRate execured without result");
                    return null;
                }
                _logger.LogInformation("GetCurrencyConverteRate completed");
                return Ok(exchangeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCurrencyConverteRate error occured", ex);
                return null;
                // throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAvailableCurrencies")]
        public async Task<ActionResult<List<Currency>>> GetAvailableCurrencies()
        {
            try
            {
                _logger.LogInformation("GetAvailableCurrencies started");
                var currencies = await _currencyRepository.GetAllCurrencies();
                _logger.LogInformation("GetAvailableCurrencies completed");
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAvailableCurrencies error occured", ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCurrencyExchangeRates/{baseCurrency}/{targetCurrency}/{startDate}/{endDate}")]
        public async Task<ActionResult<CurrencyCompareModel>> GetCurrencyExchangeRates(string baseCurrency, string targetCurrency, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                _logger.LogInformation("GetCurrencyExchangeRates started");
                var exchangeRates = await _currencyExchangeRateRepository.GetAllCurrencyExchangeRates(baseCurrency, targetCurrency, startDate, endDate);
                _logger.LogInformation("GetCurrencyExchangeRates completed");
                return Ok(exchangeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCurrencyExchangeRates error occured", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
