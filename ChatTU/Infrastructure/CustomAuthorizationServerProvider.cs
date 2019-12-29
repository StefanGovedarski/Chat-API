using ChatTU.Mappings;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ChatTU.Infrastructure
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = Security.Login(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Username or password is incorrect.");
                return;
            }
            var roles = Security.GetUserRoles(user);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, roles.Any(x => x == "ADMIN") ? "ADMIN" : "CLIENT"));

            context.Validated(identity);
        }
    }
}