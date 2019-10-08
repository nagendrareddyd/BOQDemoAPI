using BOQ.API.Constants;
using BOQ.API.Services;
using BOQ.API.Services.DTO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace BOQ.API.Tests.Services
{
    public class DemoConferenceServiceTest
    {
        private readonly IHttpClientService httpClientService;
        private readonly ILogger<IDemoConferenceService> logger;
        private readonly IDemoConferenceService demoConferenceService;
        private readonly IMemoryCache cache;

        public DemoConferenceServiceTest()
        {
            httpClientService = Substitute.For<IHttpClientService>();
            logger = Substitute.For<ILogger<IDemoConferenceService>>();
            cache = Substitute.For<IMemoryCache>();
            demoConferenceService = new DemoConferenceService(httpClientService, logger, cache);
        }

        [Fact]
        public async void GetSessionAndSpeakerCollection_Success()
        {
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl).Returns<RootObject>(GetMockSessions());
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SpeakersUrl).Returns<RootObject>(GetMockSpeakers());

            var result = await demoConferenceService.GetAllSessionsAndSpeakers();

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<SessionSpeakerCollection>(
                result);
            Assert.Equal(2, model.Sessions.Count);
        }

        [Fact]
        public async void GetSessionAndSpeakerCollection_ReturnsNull_HttpFailed()
        {
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl).Returns<RootObject>(c => { throw new Exception(""); });
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SpeakersUrl).Returns<RootObject>(GetMockSpeakers());

            var result = await demoConferenceService.GetAllSessionsAndSpeakers();

            Assert.Null(result);
        }

        [Fact]
        public async void GetSession_Success()
        {
            httpClientService.GetAsync($"{ConferenceAPIRoute.SessionUrl}101").Returns("session data discription");
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl).Returns<RootObject>(GetMockSessions());

            var result = await demoConferenceService.GetSession(101);

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Session>(
                result);
            Assert.Equal("/demo/101", model.href);
        }

        [Fact]
        public async void GetSession_ReturnsNull_HttpFailed()
        {
            httpClientService.GetAsync($"{ConferenceAPIRoute.SessionUrl}101").Returns<string>(c => { throw new Exception(""); });
            httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl).Returns<RootObject>(GetMockSessions());

            var result = await demoConferenceService.GetSession(101);

            Assert.Null(result);
        }

        private RootObject GetMockSessions()
        {
            return new RootObject()
            {
                collection = new Collection()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            href = "/demo/101"
                        },
                        new Item()
                        {
                            href = "/demo/102"
                        }
                    }
                }
            };
        }
        private RootObject GetMockSpeakers()
        {
            return new RootObject()
            {
                collection = new Collection()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            href = "/demo/101"
                        },
                        new Item()
                        {
                            href = "/demo/102"
                        }
                    }
                }
            };
        }
    }
}
