using BOQ.API.Constants;
using BOQ.API.Services.DTO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOQ.API.Services
{
    public class DemoConferenceService : IDemoConferenceService
    {
        private readonly IHttpClientService httpClientService;
        private readonly ILogger<IDemoConferenceService> logger;
        private IMemoryCache cache;

        public DemoConferenceService(IHttpClientService httpClientService, ILogger<IDemoConferenceService> logger,
            IMemoryCache memoryCache)
        {
            this.httpClientService = httpClientService;
            this.logger = logger;
            cache = memoryCache;
        }

        /// <summary>
        /// Get all sessions and speakers
        /// </summary>
        /// <returns></returns>
        public async Task<SessionSpeakerCollection> GetAllSessionsAndSpeakers()
        {
            try
            {
                var sessionResult = await httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl);
                var speakerResult = await httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SpeakersUrl);

                return new SessionSpeakerCollection()
                {
                    Sessions = sessionResult.collection?.items,
                    Speakers = speakerResult.collection?.items
                };
            }
            catch(Exception ex)
            {
                logger.LogError("Failed to get sessions and speakers", ex);
                return null;
            }
        }

        /// <summary>
        /// Get session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public async Task<Session> GetSession(int sessionId)
        {
            try
            {
                var sessionUri = $"{ConferenceAPIRoute.SessionUrl}{sessionId}";

                var sessionDescription = await httpClientService.GetAsync(sessionUri);

                var sessions = await GetSessionsFromCache();

                var session = sessions.Where(s => s.href.EndsWith("/" + sessionId.ToString(),
                                            System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (session == null)
                {
                    return null;
                }

                return new Session()
                {
                    data = session.data,
                    href = session.href,
                    links = session.links,
                    Description = sessionDescription
                };
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get session data", ex);
                return null;
            }
        }

        private async Task<List<Item>> GetSessionsFromCache()
        {
            List<Item> cacheSessions;
            if (!cache.TryGetValue(CacheKey.Sessions, out cacheSessions))
            {
                // Key not in cache, so get data.
                var rootObject = await httpClientService.GetAsync<RootObject>(ConferenceAPIRoute.SessionsUrl);
                cacheSessions = rootObject.collection?.items;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // TODO:: configure this value

                // Save data in cache.
                cache.Set(CacheKey.Sessions, cacheSessions, cacheEntryOptions);
            }
            return cacheSessions;
        }
    }
}
