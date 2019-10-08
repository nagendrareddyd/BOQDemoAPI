using BOQ.API.Controllers;
using BOQ.API.Services;
using BOQ.API.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BOQ.API.Tests.Controllers
{
    public class ConferenceControllerTest
    {
        private readonly ConferenceController conferenceController;
        private readonly IDemoConferenceService demoConferenceService;

        public ConferenceControllerTest()
        {
            demoConferenceService = Substitute.For<IDemoConferenceService>();
            conferenceController = new ConferenceController(demoConferenceService);
        }

        [Fact]
        public async void GetSessionsAndSpeakers_Success()
        {
            SessionSpeakerCollection sessionSpeakerCollection = GetMockSessionsAndSpeakerCollection();
            demoConferenceService.GetAllSessionsAndSpeakers().Returns(sessionSpeakerCollection);

            var result = await conferenceController.GetAllSessionsAndSpeakers();

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<SessionSpeakerCollection>(
                viewResult.Value);
            Assert.Equal(3, model.Sessions.Count);
        }
        
        [Fact]
        public async void GetSessionsAndSpeakers_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            demoConferenceService.GetAllSessionsAndSpeakers().Returns((SessionSpeakerCollection)null);

            //act
            var result = await conferenceController.GetAllSessionsAndSpeakers();

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestObjectResult.Value);
        }

        [Fact]
        public async void GetSession_Success()
        {
            var mockSession = GetMockSession();
            demoConferenceService.GetSession(101).Returns(mockSession);

            var result = await conferenceController.GetSession(101);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Session>(
                viewResult.Value);
            Assert.Equal("demo/101", model.href);
        }

        [Fact]
        public async void GetSession_ReturnsBadRequestResult_WhenSessionNotFound()
        {
            //Arrange
            demoConferenceService.GetSession(101).Returns((Session)null);

            //act
            var result = await conferenceController.GetSession(101);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request for session id 101", badRequestObjectResult.Value);
        }

        private Session GetMockSession()
        {
            return new Session()
            {
                Description = "Session1",
                href = "demo/101"
            };
        }

        private SessionSpeakerCollection GetMockSessionsAndSpeakerCollection()
        {
            return new SessionSpeakerCollection()
            {
                Sessions = new List<Item>()
                {
                    new Item()
                    {
                        href = "link1"
                    },
                    new Item()
                    {
                        href = "link2"
                    },
                    new Item()
                    {
                        href = "link3"
                    }
                },

                Speakers = new List<Item>()
                {
                    new Item()
                    {
                        href = "link1"
                    },
                    new Item()
                    {
                        href = "link2"
                    },
                    new Item()
                    {
                        href = "link3"
                    }
                }
            };
        }
    }
}
