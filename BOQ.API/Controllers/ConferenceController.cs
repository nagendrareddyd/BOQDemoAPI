using BOQ.API.Services;
using BOQ.API.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BOQ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConferenceController : ControllerBase
    {
        private readonly IDemoConferenceService _demoConferenceService;
        public ConferenceController(IDemoConferenceService demoConferenceService)
        {
            _demoConferenceService = demoConferenceService;
        }

        /// <summary>
        /// Get All Sessions and speakers
        /// </summary>
        /// <returns>SessionsAndSpeakersCollection</returns>
        [HttpGet]
        [Route("allSessionsAndSpeakers")]
        [ProducesResponseType(typeof(SessionSpeakerCollection), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSessionsAndSpeakers()
        {
            var result = await _demoConferenceService.GetAllSessionsAndSpeakers();
            if(result == null)
            {
                return BadRequest("Invalid request");
            }
            return Ok(result);
        }

        /// <summary>
        /// Get session using session id
        /// </summary>
        /// <param name="id">session id</param>
        /// <returns>session</returns>
        [HttpGet]
        [Route("session/{id}")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSession(int id)
        {
            var result = await _demoConferenceService.GetSession(id);
            if (result == null)
            {
                return BadRequest($"Invalid request for session id {id}");
            }
            return Ok(result);
        }
    }
}