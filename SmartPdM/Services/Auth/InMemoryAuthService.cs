//using Android.Renderscripts;
using System.Collections.Concurrent;

namespace SmartPdM.Services.Auth;

public class InMemoryAuthService : IAuthService
{
    private static readonly ConcurrentDictionary<string, (string Hash, string Name, Guid Id)> _users = new();
    private AuthUser? _current;

    public Task<AuthUser> SignUpAsync(SignUpRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            throw new ArgumentException("이메일/비밀번호를 입력하세요.");

        if (_users.ContainsKey(req.Email.ToLower()))
            throw new InvalidOperationException("이미 가입된 이메일입니다.");

        var id = Guid.NewGuid();
        _users[req.Email.ToLower()] = (BCrypt.Net.BCrypt.HashPassword(req.Password), req.DisplayName, id);

        var user = new AuthUser(id, req.Email, req.DisplayName, Guid.NewGuid().ToString("N"));
        _current = user;
        return Task.FromResult(user);
    }

    public Task<AuthUser> SignInAsync(SignInRequest req)
    {
        if (_users.TryGetValue(req.Email.ToLower(), out var info) &&
            BCrypt.Net.BCrypt.Verify(req.Password, info.Hash))
        {
            _current = new AuthUser(info.Id, req.Email, info.Name, Guid.NewGuid().ToString("N"));
            return Task.FromResult(_current);
        }
        throw new UnauthorizedAccessException("이메일 또는 비밀번호가 올바르지 않습니다.");
    }

    public Task SignOutAsync() { _current = null; return Task.CompletedTask; }
    public Task<AuthUser?> GetCurrentAsync() => Task.FromResult(_current);
}
