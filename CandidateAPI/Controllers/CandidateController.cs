
using Microsoft.AspNetCore.Mvc;
using CandidateAPI.DTOs;
using CandidateAPI.Models;
using CandidateAPI.Services.CoreServices;

namespace CandidateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _service;

        public CandidateController(ICandidateService service)
        {
            _service = service;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertCandidate([FromBody] CandidateRequestDTO candidateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.UpsertCandidateAsync(candidateDto);
            return Ok($"Candidate {result.OperationType} successfully.");
        }
    }
}
