using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.ServiceModel
{
    public class CurrencyRateModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("base")]
        public string? SourceCurrency { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, string>? ConvertionRates { get; set; }

        [JsonProperty("date")]
        [DataType(DataType.Date)]
        public DateTime ConvertionDate { get; set; }
        [JsonProperty("error")]
        public Error? Error { get; set; }

    }
}


