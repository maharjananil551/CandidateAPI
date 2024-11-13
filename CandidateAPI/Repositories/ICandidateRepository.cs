
using CandidateAPI.Models;

namespace CandidateAPI.Repositories
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByEmailAsync(string email);
        Task AddAsync(Candidate candidate);
        //Task UpdateAsync(Candidate candidate);
        void Update(Candidate candidate);
        Task SaveChangesAsync();
    }
}
