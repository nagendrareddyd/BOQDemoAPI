using BOQ.API.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services
{
    public interface IDemoConferenceService
    {
        Task<SessionSpeakerCollection> GetAllSessionsAndSpeakers();
        Task<Session> GetSession(int sessionId);
    }
}
