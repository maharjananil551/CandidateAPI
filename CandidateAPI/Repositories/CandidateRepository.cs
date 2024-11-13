using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CandidateAPI.Data;
using CandidateAPI.Models;

namespace CandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetByEmailAsync(string email)
        {
            return await _context.Candidates.SingleOrDefaultAsync(c => c.Email == email)??new Candidate();
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
        }


        public void Update(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
        }
        //public async Task UpdateAsync(Candidate candidate)
        //{
        //     _context.Candidates.Update(candidate);
        //}

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}