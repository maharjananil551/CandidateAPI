using CandidateAPI.DTOs;
using CandidateAPI.Models;

namespace CandidateAPI.Services.CoreServices
{
    public interface ICandidateService
    {
        Task<CandidateResponseDTO> UpsertCandidateAsync(CandidateRequestDTO candidateDto);
    }
}