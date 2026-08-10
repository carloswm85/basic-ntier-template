namespace CleanArchitectureTemplate.API.Contracts.Identity.IdentityContractDtos;

public class UserInformationResponse
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required IList<string> Roles { get; set; }
}
