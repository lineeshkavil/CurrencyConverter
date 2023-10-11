using CurrencyConverter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Service.Contracts
{
    public interface IUserService
    {
        Task<bool> GetUser(string userName, string password);
    }
}
