
namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string? email, string v1, string v2);
    }
}