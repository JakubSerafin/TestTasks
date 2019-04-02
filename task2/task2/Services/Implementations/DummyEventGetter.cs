using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{


    public class SimpleSyncDate : ISyncDate
    {
        public DateTime? SyncDate
        {
            get;
            set;
        }
    }
    public class ApiEventLocalManager: IEventGetter, IPagedEventGeter
    {
        private IAsyncEventGetter emptyCollectionReciever;
        private IAsyncEventGetter filledCollectionReciever;
        private ISyncDate lastSyncDateSeter;
        private List<EventObject> _events = new List<EventObject>();

        public ApiEventLocalManager(IAsyncEventGetter emptyCollectionReciever, IAsyncEventGetter filledCollectionReciever, ISyncDate lastSyncDateSeter)
        {
            this.emptyCollectionReciever = emptyCollectionReciever;
            this.filledCollectionReciever = filledCollectionReciever;
            this.lastSyncDateSeter = lastSyncDateSeter;
        }

        public IList<EventObject> GetEventNotifications()
        {
            return _events;
        }


       

        public async Task GetFromRemote()
        {
            IList<EventObject> events;
            if (lastSyncDateSeter.SyncDate == null)
            {
                events = await emptyCollectionReciever.GetEventNotifications();
            }
            else
            {
               events = await filledCollectionReciever.GetEventNotifications();
            }

            _events.AddRange(events);
            lastSyncDateSeter.SyncDate = DateTime.Now;
        }

        public IList<EventObject> GetPage(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }
    }

    public class EventsGetterBacgroundService
    {
        private readonly Action _actionToCall;
        private readonly IServiceBlocker _blocker;
        private CancellationToken _token;


        public EventsGetterBacgroundService(Action action, CancellationToken token, IServiceBlocker blocker = null)
        {
            _actionToCall = action;
            _token = token;
            _blocker = blocker;

        }

        public async Task Run()
        {
            await Task.Run(() =>
            {
                while (!_token.IsCancellationRequested)
                {
                    if (_blocker?.CanProcess() ?? true)
                    {
                        _actionToCall.Invoke();
                    }
                }
            });
        }
    }

    public class UnixEpochWithMilisecondsConventer : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = (long)reader.Value;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    //public class RealEventGetter: 

    public class DummyEventGetter : IEventGetter
    {
        public IList<EventObject> GetEventNotifications()
        {
            return JsonConvert.DeserializeObject<EventObject[]>(_json,new UnixEpochWithMilisecondsConventer()).ToList();
        }

        private EventObject FromJson(string json)
        {
            return JsonConvert.DeserializeObject<EventObject>(json);
        }

        private string _json = @"                [{
                    ""category"": ""Proces Interwencyjny"",
                    ""city"": ""Warszawa"",
                    ""subcategory"": ""Drogi"",
                    ""district"": ""Praga Południe"",
                    ""aparmentNumber"": null,
                    ""street2"": ""Kapelanów AK 1, 04-046 Praga-Południe"",
                    ""notificationType"": ""INCIDENT"",
                    ""createDate"": 1553461418000,
                    ""siebelEventId"": ""1-IJIMVU"",
                    ""source"": ""MOBILE"",
                    ""yCoordOracle"": 5788374.393079,
                    ""street"": null,
                    ""deviceType"": ""IOS"",
                    ""statuses"": [
                        {
                            ""status"": ""Nowe"",
                            ""description"": null,
                            ""changeDate"": 1553497209000
                        },
                        {
                            ""status"": ""W trakcie"",
                            ""description"": null,
                            ""changeDate"": 1553497336000
                        }
                    ],
                    ""xCoordOracle"": 7505831.012543,
                    ""notificationNumber"": ""135285/19"",
                    ""yCoordWGS84"": 52.2289319442578,
                    ""event"": ""Uszkodzenie lub brak urządzeń różnych - zamontowanych w jezdniach, chodnikach"",
                    ""xCoordWGS84"": 21.0853462197776
                },
                {
                    ""category"": ""Proces Interwencyjny"",
                    ""city"": ""Warszawa"",
                    ""subcategory"": ""Drogi"",
                    ""district"": ""Praga Południe"",
                    ""aparmentNumber"": null,
                    ""street2"": ""PARYSKA 8, 03-954 Praga-Południe"",
                    ""notificationType"": ""INCIDENT"",
                    ""createDate"": 1553462972000,
                    ""siebelEventId"": ""1-IJG6CJ"",
                    ""source"": ""MOBILE"",
                    ""yCoordOracle"": 5788125.920698,
                    ""street"": null,
                    ""deviceType"": ""IOS"",
                    ""statuses"": [
                        {
                            ""status"": ""Nowe"",
                            ""description"": null,
                            ""changeDate"": 1553462973000
                        },
                        {
                            ""status"": ""W trakcie"",
                            ""description"": null,
                            ""changeDate"": 1553466998000
                        }
                    ],
                    ""xCoordOracle"": 7504070.58216,
                    ""notificationNumber"": ""135308/19"",
                    ""yCoordWGS84"": 52.2267145667074,
                    ""event"": ""Źle zaparkowane pojazdy"",
                    ""xCoordWGS84"": 21.0595765271323
                },
                {
                    ""category"": ""Proces Interwencyjny"",
                    ""city"": ""Warszawa"",
                    ""subcategory"": ""Drogi"",
                    ""district"": ""Żoliborz"",
                    ""aparmentNumber"": null,
                    ""street2"": ""PL. C. NIEMENA 1, 01-748 Żoliborz"",
                    ""notificationType"": ""INCIDENT"",
                    ""createDate"": 1553461091000,
                    ""siebelEventId"": ""1-IJGDN2"",
                    ""source"": ""MOBILE"",
                    ""yCoordOracle"": 5791367.204794,
                    ""street"": null,
                    ""deviceType"": ""IOS"",
                    ""statuses"": [
                        {
                            ""status"": ""Nowe"",
                            ""description"": null,
                            ""changeDate"": 1553461120000
                        },
                        {
                            ""status"": ""W trakcie"",
                            ""description"": null,
                            ""changeDate"": 1553463338000
                        }
                    ],
                    ""xCoordOracle"": 7497712.319148,
                    ""notificationNumber"": ""135282/19"",
                    ""yCoordWGS84"": 52.2558564859712,
                    ""event"": ""Zmiana organizacji ruchu"",
                    ""xCoordWGS84"": 20.9664958534345
                },
                {
                    ""category"": ""Proces Interwencyjny"",
                    ""city"": ""Warszawa"",
                    ""subcategory"": ""Porządek i bezpieczeństwo"",
                    ""district"": ""Rembertów"",
                    ""aparmentNumber"": null,
                    ""street2"": null,
                    ""notificationType"": ""INCIDENT"",
                    ""createDate"": 1553461315000,
                    ""siebelEventId"": ""1-IJGDPB"",
                    ""source"": ""PORTAL"",
                    ""yCoordOracle"": 5791472.932271,
                    ""street"": ""Chełmżyńska"",
                    ""deviceType"": ""UNKNOWN"",
                    ""statuses"": [
                        {
                            ""status"": ""Nowe"",
                            ""description"": null,
                            ""changeDate"": 1553461551000
                        },
                        {
                            ""status"": ""W trakcie"",
                            ""description"": null,
                            ""changeDate"": 1553466567000
                        }
                    ],
                    ""xCoordOracle"": 7508798.951228,
                    ""notificationNumber"": ""135283/19"",
                    ""yCoordWGS84"": 52.256741156756455,
                    ""event"": ""Zanieczyszczenie powietrza"",
                    ""xCoordWGS84"": 21.128867308809276
                },
                {
                    ""category"": ""Proces Interwencyjny"",
                    ""city"": ""Warszawa"",
                    ""subcategory"": ""Drogi"",
                    ""district"": ""Praga Południe"",
                    ""aparmentNumber"": null,
                    ""street2"": ""NIEKŁAŃSKA 33, 03-924 Praga-Południe"",
                    ""notificationType"": ""INCIDENT"",
                    ""createDate"": 1553462881000,
                    ""siebelEventId"": ""1-IJGWL9"",
                    ""source"": ""MOBILE"",
                    ""yCoordOracle"": 5789201.487603,
                    ""street"": null,
                    ""deviceType"": ""IOS"",
                    ""statuses"": [
                        {
                            ""status"": ""Nowe"",
                            ""description"": null,
                            ""changeDate"": 1553462881000
                        },
                        {
                            ""status"": ""W trakcie"",
                            ""description"": null,
                            ""changeDate"": 1553466876000
                        }
                    ],
                    ""xCoordOracle"": 7504121.94253,
                    ""notificationNumber"": ""135306/19"",
                    ""yCoordWGS84"": 52.2363810513781,
                    ""event"": ""Źle zaparkowane pojazdy"",
                    ""xCoordWGS84"": 21.0603413358519
                }]";
    }
}
