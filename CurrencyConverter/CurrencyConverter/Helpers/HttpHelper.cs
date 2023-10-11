using CurrencyConverter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Repositories.Helpers
{
    public  class HttpHelper:IHttpHelper
    {
        public async Task<string> GetHttpRequest(string url)
        {
            string content = string.Empty;

            using (var client = new HttpClient()) 
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                }
            }
            return content;
        }
    }
}
