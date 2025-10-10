using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Web.Auth
{

    using Microsoft.AspNetCore.Components.Authorization;
    using System.Security.Claims;



    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public void SetUser(Usuario usuario)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email)
        };

            foreach (var rol in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var identity = new ClaimsIdentity(claims, "customAuth"); // authType != null
            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity()); // vuelve a anónimo
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }
 
    }


}
