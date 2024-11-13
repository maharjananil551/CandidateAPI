using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using CandidateAPI.DTOs;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.Services.CoreServices;

namespace CandidateAPI.Tests
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _mockRepository;
        private readonly Mock<ICacheService> _mockCache;
        private readonly IMapper _mapper;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            _mockRepository = new Mock<ICandidateRepository>();
            _mockCache = new Mock<ICacheService>();

            var config = new MapperConfiguration(cfg =>
            {
                // Assuming we have the necessary mappings in place
                cfg.CreateMap<Candidate, CandidateResponseDTO>();
                cfg.CreateMap<CandidateRequestDTO, Candidate>();
            });

            _mapper = config.CreateMapper();
            _candidateService = new CandidateService(_mockRepository.Object, _mapper, _mockCache.Object);
        }

        [Fact]
        public async Task UpsertCandidateAsync_WhenCandidateDoesNotExist_ShouldInsertAndReturnResponse()
        {
            // Arrange
            var candidateDto = new CandidateRequestDTO
            {
                Email = "imanil@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var candidate = new Candidate
            {
                Email = "imanil@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            _mockRepository.Setup(x => x.GetByEmailAsync(candidateDto.Email)).ReturnsAsync((Candidate)null);
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Candidate>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _candidateService.UpsertCandidateAsync(candidateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Inserted", result.OperationType);
            Assert.Equal("imanil@example.com", result.Email);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Candidate>()), Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpsertCandidateAsync_WhenCandidateExists_ShouldUpdateAndReturnResponse()
        {
            // Arrange
            var candidateDto = new CandidateRequestDTO
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var existingCandidate = new Candidate
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            _mockRepository.Setup(x => x.GetByEmailAsync(candidateDto.Email)).ReturnsAsync(existingCandidate);
            _mockRepository.Setup(x => x.Update(It.IsAny<Candidate>())).Verifiable();
            _mockRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _candidateService.UpsertCandidateAsync(candidateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.OperationType);
            Assert.Equal("test@example.com", result.Email);
            _mockRepository.Verify(x => x.Update(It.IsAny<Candidate>()), Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCandidateAsync_WhenCacheExists_ShouldReturnCandidateFromCache()
        {
            // Arrange
            var email = "cache_test@example.com";
            var cachedCandidate = new Candidate
            {
                Email = email,
                FirstName = "John",
                LastName = "Doe"
            };

            _mockCache.Setup(x => x.Exists(email)).Returns(true);
            _mockCache.Setup(x => x.Get<Candidate>(email)).Returns(cachedCandidate);

            // Act
            var result = await _candidateService.GetCandidateAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            _mockCache.Verify(x => x.Exists(email), Times.Once);
            _mockCache.Verify(x => x.Get<Candidate>(email), Times.Once);
            _mockRepository.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetCandidateAsync_WhenCacheDoesNotExist_ShouldReturnCandidateFromRepository()
        {
            // Arrange
            var email = "cache_test@example.com";
            var candidate = new Candidate
            {
                Email = email,
                FirstName = "John",
                LastName = "Doe"
            };

            _mockCache.Setup(x => x.Exists(email)).Returns(false);
            _mockRepository.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(candidate);
           // _mockCache.Setup(x => x.Set(email, candidate));

            // Act
            var result = await _candidateService.GetCandidateAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            _mockCache.Verify(x => x.Exists(email), Times.Once);
            _mockRepository.Verify(x => x.GetByEmailAsync(email), Times.Once);
            _mockCache.Verify(x => x.Set(email, candidate,null), Times.Once);
        }

        [Fact]
        public async Task UpsertCandidateAsync_WhenEmailIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            var candidateDto = new CandidateRequestDTO
            {
                Email = null, // Invalid email
                FirstName = "John",
                LastName = "Doe"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _candidateService.UpsertCandidateAsync(candidateDto));

            // Assert
            Assert.Equal("Email is required (Parameter 'Email')", exception.Message);
        }
    }
}
