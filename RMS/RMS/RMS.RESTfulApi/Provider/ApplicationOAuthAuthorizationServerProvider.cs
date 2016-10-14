using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using RMS.RESTfulApi.Models;

namespace RMS.RESTfulApi.Provider
{
    public class ApplicationOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new RmsAuthContext())))
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var roles = await userManager.GetRolesAsync(user.Id);

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("userName", user.UserName));

                if (roles.Count > 0)
                    identity.AddClaim(new Claim(ClaimTypes.Role, roles.FirstOrDefault()));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "username", user.UserName
                    }

                });

                if (roles.Count > 0)
                   props.Dictionary.Add("role", roles.FirstOrDefault());

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}