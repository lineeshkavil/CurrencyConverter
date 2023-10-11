using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Entities
{
    public class CurrencyExchangeRate
    {
        [Key]
        public int Id;
        public int SourceCurrencyId { get; set; }

        [ForeignKey("SourceCurrencyId")]
        public virtual Currency SourceCurrency { get; set; }
        public int TargetCurrencyId { get; set; }

        [ForeignKey("TargetCurrencyId")]
        public virtual Currency TargetCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime Date { get; set; }
    }
}
