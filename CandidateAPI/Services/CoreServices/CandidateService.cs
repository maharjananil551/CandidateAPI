// Services/CandidateService.cs
using AutoMapper;
using CandidateAPI.DTOs;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.Services.CoreServices;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    public CandidateService(ICandidateRepository repository, IMapper mapper, ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    // CandidateService.cs

    public async Task<Candidate> GetCandidateAsync(string email)
    {
        // Check if candidate is in cache
        if (_cache.Exists(email))
        {
            return _cache.Get<Candidate>(email);  // Return cached candidate data
        }

        // If not in cache, retrieve from repository
        var candidate = await _repository.GetByEmailAsync(email);
        if (candidate != null)
        {
            //  var result = _mapper.Map<Candidate>(candidate);
            _cache.Set(email, candidate);  // Cache for 5 minutes
            return candidate;
        }

        return null;
    }

    public async Task<CandidateResponseDTO> UpsertCandidateAsync(CandidateRequestDTO candidateDto)
    {
        if (string.IsNullOrEmpty(candidateDto.Email))
        {
            throw new ArgumentException("Email is required", nameof(candidateDto.Email));
        }
        // Retrieve candidate by email
        var existingCandidate = await GetCandidateAsync(candidateDto.Email);
        string operationType;

        if (existingCandidate == null)
        {
            // Insert new candidate
            var newCandidate = _mapper.Map<Candidate>(candidateDto);
            await _repository.AddAsync(newCandidate);
            await _repository.SaveChangesAsync();
            existingCandidate = newCandidate;
            operationType = "Inserted";
        }
        else
        {
            // Update existing candidate
            _mapper.Map(candidateDto, existingCandidate);
            _repository.Update(existingCandidate);
            await _repository.SaveChangesAsync();
            operationType = "Updated";
        }

        // Map the updated candidate to the response DTO
        var response = _mapper.Map<CandidateResponseDTO>(existingCandidate);
        response.OperationType = operationType;

        // Update the cache with the latest candidate data
        _cache.Set(candidateDto.Email, existingCandidate);

        return response;
    }

}
