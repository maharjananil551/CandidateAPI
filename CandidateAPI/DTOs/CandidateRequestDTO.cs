using System.ComponentModel.DataAnnotations;

namespace CandidateAPI.DTOs
{
    public class CandidateRequestDTO
    {
        [Required]
        public string? FirstName { get; set; } 

        [Required]
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        public string? PreferredCallTime { get; set; }

        [Url]
        public string? LinkedInProfile { get; set; }

        [Url]
        public string? GitHubProfile { get; set; }

        [Required]
        public string? Comments { get; set; }
    }
}
