using Nancy.Authentication.Stateless;
using Nancy.Security;
using System;
using System.Linq;

namespace EchoService.Auth
{
    public static class SimpleOAuthConfiguration 
    {
        private const string ValidBearerToken = "NonPublicBearerToken";

        public static StatelessAuthenticationConfiguration GetOAuthConfiguration() =>
            new StatelessAuthenticationConfiguration(ctx =>
            {
                var authorizationHeader = ctx.Request.Headers["Authorization"];
                if (authorizationHeader == null || authorizationHeader.Count() == 0 || authorizationHeader.Count() > 1)
                    return null;

                var authValue = authorizationHeader.First();
                var pieces = authValue.Split(new char[] { ' ' }, StringSplitOptions.None);
                if (pieces.Length != 2)
                    return null;

                switch (pieces[0].ToLower())
                {
                    case "bearer":
                        return ValidateBearer(pieces[1]);
                    case "basic":
                        return ValidateBasic(pieces[1]);
                    default:
                        return null;
                }
            });

        private static IUserIdentity ValidateBearer(string bearerToken) =>
            string.Equals(ValidBearerToken, bearerToken) ? new UserIdentity() : null;

        private static IUserIdentity ValidateBasic(string basicAuthValue) =>
            throw new NotImplementedException("Basic Auth is not implemented yet");
    }
}