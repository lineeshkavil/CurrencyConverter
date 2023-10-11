using CurrencyConverter.Entities;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.DataServices
{
    public class UserService: IUserService
    {
        private readonly CurrencyDbContext _context;


        public UserService(CurrencyDbContext context)
        {
             _context = context;
        }
        public async Task<bool> GetUser(string userName, string password)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName && x.Password == password);

        }
    }
}
