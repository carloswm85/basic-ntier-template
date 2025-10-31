namespace BasicNtierTemplate.Service.Contracts
{
    public class RegistrationResult
    {
        public string UserId { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsSuccess { get; internal set; }
        public List<string> Errors { get; internal set; }

        public RegistrationResult() { }

        public RegistrationResult(string userId, bool isEmailSent)
        {
            UserId = userId;
            IsEmailSent = isEmailSent;
        }
    }
}
