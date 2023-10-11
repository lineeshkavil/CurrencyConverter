using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.ServiceModel
{
    public class CurrencyApiModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("symbols")]
        public Dictionary<string, string>? Currencies { get; set; }
    }
}
