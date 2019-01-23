using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AuthAPI
{
    internal class SimpleAuthorizationServerProvider : OpenIdConnectServerProvider
    {
        public static ConcurrentDictionary<string,string> refreshTokenList = new ConcurrentDictionary<string,string>();


        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {

            var isPasswordGrantType = context.Request.IsPasswordGrantType();
             
            if (isPasswordGrantType)
            {

                var userName = context.Request.Username;
                var password = context.Request.Password;

                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                {
                    context.Validate();
                    return Task.FromResult<object>(null);
                }
            }

            context.Reject(
                        error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                        description: "Only authorization code, refresh token, and token grant types are accepted by this authorization server."
                    );

            return Task.FromResult<object>(null);

        }

        public override Task ValidateAuthorizationRequest(ValidateAuthorizationRequestContext context)
        {
            return base.ValidateAuthorizationRequest(context);
        }

        public override Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            ClaimsIdentity identity = new ClaimsIdentity();

            identity.AddClaim(
                new Claim(OpenIdConnectConstants.Claims.Username, context.Request.Username));

            identity.AddClaim(
               new Claim(OpenIdConnectConstants.Claims.Subject, context.Request.ClientId));


            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties(), OpenIdConnectServerDefaults.AuthenticationScheme);
            ICollection<string> scopesToAdd = new List<string>()
            {
                /* If  you've chosen to add an OpenId token to your destinations, be sure to include the OpenIdCOnnectConstants.Scopes.OpenId in this list */
                //OpenIdConnectConstants.Scopes.OpenId, // Lets our requesting clients know that an OpenId Token was generated with the original request.
            };

            scopesToAdd.Add(OpenIdConnectConstants.Scopes.OfflineAccess);

            ticket.SetScopes(scopesToAdd);
            context.Validate(ticket);
            return Task.CompletedTask;
        }

        public override Task ApplyTokenResponse(ApplyTokenResponseContext context)
        {
            var passwordGrandType = context.Request.IsPasswordGrantType();

            if (passwordGrandType)
            {
                refreshTokenList.AddOrUpdate(context.Response.RefreshToken,context.Request.ClientId,(string oldToken,string oldClientId)=> oldClientId);
            }

            return Task.FromResult<object>(null);
        }


       
    }
}