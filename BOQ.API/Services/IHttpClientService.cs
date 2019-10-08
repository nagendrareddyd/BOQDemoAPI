using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string uri);
        Task<string> GetAsync(string uri);
    }
}
