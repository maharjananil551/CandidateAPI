public class CandidateResponseDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string LinkedInProfile { get; set; }
    public string GitHubProfile { get; set; }
    public string OperationType { get; set; }  // "Inserted" or "Updated"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}