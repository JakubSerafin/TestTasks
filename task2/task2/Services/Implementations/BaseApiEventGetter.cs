using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class BaseApiEventGetter : IAsyncEventGetter
    {
        private const string Action = "19115store_getNotifications";
        private const string URL = @"https://api.um.warszawa.pl/api/action/";
        private readonly string _secretKey;

        public BaseApiEventGetter(string secretKey)
        {
            _secretKey = secretKey;
        }

        public async Task<IList<EventObject>> GetEventNotifications()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);
            var queryParams = new Dictionary<string, string>
             {
                 {"id", "28dc65ad-fff5-447b-99a3-95b71b4a7d1e"},
                 {"apiKey", _secretKey}
             };
            var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(Action, queryParams));

            var result = await httpClient.SendAsync(request);
            var content = await result.Content.ReadAsStringAsync();

            var colection = JsonConvert.DeserializeObject<List<EventObject>>(content);
            return colection;
        }
    }


    //public class RealEventGetter: 
}
