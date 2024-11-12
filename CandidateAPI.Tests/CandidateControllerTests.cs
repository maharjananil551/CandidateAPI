// CandidateControllerTests.cs
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CandidateAPI.Controllers;
using CandidateAPI.DTOs;
using CandidateAPI.Mapping;
using CandidateAPI.Services.CoreServices;  // Import your main AutoMapper profile

namespace CandidateAPI.Tests.Controllers
{
    public class CandidateControllerTests
    {
        private readonly Mock<ICandidateService> _serviceMock;
        private readonly CandidateController _controller;

        public CandidateControllerTests()
        {
            _serviceMock = new Mock<ICandidateService>();


            _controller = new CandidateController(_serviceMock.Object);
        }

        [Fact]
        public async Task UpsertCandidate_ShouldReturnOk_WhenCandidateIsCreated()
        {
            // Arrange
            var candidateDto = new CandidateRequestDTO { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            var responseDto = new CandidateResponseDTO { FirstName = "John", LastName = "Doe", Email = "john@example.com" };

            _serviceMock.Setup(s => s.UpsertCandidateAsync(candidateDto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.UpsertCandidate(candidateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CandidateResponseDTO>(okResult.Value);
            Assert.Equal(candidateDto.Email, returnValue.Email);
        }

        [Fact]
        public async Task UpsertCandidate_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var candidateDto = new CandidateRequestDTO { FirstName = "", LastName = "", Email = "invalid-email" };
            _controller.ModelState.AddModelError("Email", "Invalid email");

            // Act
            var result = await _controller.UpsertCandidate(candidateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
