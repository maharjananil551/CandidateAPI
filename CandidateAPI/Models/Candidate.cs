
using System.ComponentModel.DataAnnotations;

namespace CandidateAPI.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string PreferredCallTime { get; set; }

        public string LinkedInProfile { get; set; }

        public string GitHubProfile { get; set; }

        public string Comments { get; set; }
    }

}
