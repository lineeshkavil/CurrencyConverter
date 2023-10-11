using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Entities
{
    public class Currency
    {
        [Key]
        public int CurrencyId;
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }
    }
}
