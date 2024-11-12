// Mapping/MappingProfile.cs
using AutoMapper;
using CandidateAPI.DTOs;
using CandidateAPI.Models;

namespace CandidateAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CandidateRequestDTO, Candidate>();
            CreateMap<Candidate, CandidateResponseDTO>();
        }
    }
}
