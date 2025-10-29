using BasicNtierTemplate.Service.Services.Interfaces;

namespace BasicNtierTemplate.Service.Services
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendEmailAsync(string? email, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
