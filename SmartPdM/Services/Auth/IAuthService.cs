namespace SmartPdM.Services.Auth;

public record SignUpRequest(string Email, string Password, string DisplayName);
public record SignInRequest(string Email, string Password);
public record AuthUser(Guid Id, string Email, string DisplayName, string Token);

public interface IAuthService
{
    Task<AuthUser> SignUpAsync(SignUpRequest req);
    Task<AuthUser> SignInAsync(SignInRequest req);
    Task SignOutAsync();
    Task<AuthUser?> GetCurrentAsync();
}
