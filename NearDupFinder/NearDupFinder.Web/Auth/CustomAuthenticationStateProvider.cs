 
namespace NearDupFinder.Web.Auth
{
     

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _localStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

            if (userSession == null)
                return new AuthenticationState(_anonymous);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Email, userSession.Email) 
            };
            claims.AddRange(userSession.Roles.Select(rol => new Claim(ClaimTypes.Role, rol)));
    
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));

            return new AuthenticationState(claimsPrincipal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await _localStorage.SetAsync("UserSession", userSession);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Email, userSession.Email) 
            };
            claims.AddRange(userSession.Roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
        }
        else
        {
            await _localStorage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
 
public class UserSession
{
    public string UserName { get; set; }
    public string Email { get; set; } 
    public List<string> Roles { get; set; } = new();
}

}
