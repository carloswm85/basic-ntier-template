namespace BasicNtierTemplate.Service.Contracts
{
    public class RegistrationResult
    {
        public string UserId { get; set; }
        public bool EmailSent { get; set; }
        public bool Success { get; internal set; }
        public List<string> Errors { get; internal set; }

        public RegistrationResult() { }

        public RegistrationResult(string userId, bool emailSent)
        {
            UserId = userId;
            EmailSent = emailSent;
        }
    }
}
