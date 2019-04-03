using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class ApiCaller
    {
        private const string URL = @"https://api.um.warszawa.pl/api/action/";
        private readonly string _secretKey;

        public ApiCaller(string secretKey)
        {
            _secretKey = secretKey;
        }

        private string BuildQuery(string action, Dictionary<string, string> additionalParams)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"id", "28dc65ad-fff5-447b-99a3-95b71b4a7d1e"},
                {"apikey", _secretKey}
            };
            if (additionalParams != null)
            {
                foreach (var keyValuePair in additionalParams)
                {
                    queryParams[keyValuePair.Key] = keyValuePair.Value;

                }
            }

            return QueryHelpers.AddQueryString(action, queryParams);
        }

        public async Task<T> Call<T>(string action, Dictionary<string, string> additionalParams = null)
        {
            var query = BuildQuery(action, additionalParams);
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);

            var request = new HttpRequestMessage(HttpMethod.Get, BuildQuery(action,additionalParams));

            var result = await httpClient.SendAsync(request);
            var content = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<T>(content, new UnixEpochWithMilisecondsConventer());
            return response;
        }
    }

    public class TimePeriodApiEventGetter : IAsyncEventGetter
    {
        private const string Action = "19115store_getNotificationsForDate";
        private readonly ApiCaller caller;
        private readonly ISyncDate syncDate;

        public TimePeriodApiEventGetter(ApiCaller caller, ISyncDate syncDate)
        {
            this.caller = caller;
            this.syncDate = syncDate;
        }

        public async Task<IList<EventObject>> GetEventNotifications()
        {
            var datesArgs = new Dictionary<string, string>
            {
                {"dateFrom", syncDate.SyncDate.Value.ToUnixTimeMilliseconds().ToString() },
                {"dateTo", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
            };
            var response = await caller.Call<EventRequestResponse>(Action, datesArgs);

            return response.result.success ? response.result.result.notifications : new List<EventObject>();
        }
    }

    public class BaseApiEventGetter : IAsyncEventGetter
    {
        private const string Action = "19115store_getNotifications";
        private readonly ApiCaller caller;

        public BaseApiEventGetter(ApiCaller caller)
        {
            this.caller = caller;
        }

        public async Task<IList<EventObject>> GetEventNotifications()
        {
            var response = await caller.Call<EventRequestResponse>(Action);
            var isSuccess = response.result?.success; 
            return (isSuccess.HasValue && isSuccess.Value)? response.result.result.notifications: new List<EventObject>();
        }
    }


    //public class RealEventGetter: 
}
