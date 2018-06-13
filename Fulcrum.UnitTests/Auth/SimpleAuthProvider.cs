using System;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests.Auth
{
    public class SimpleAuthProvider : IAuthenticationProvider
    {
        public string BearerToken => "NonPublicBearerToken";

        public Task<Tuple<string, string>> GetAuthorizationHeader() =>
            Task.FromResult(Tuple.Create("Authorization", $"Bearer {BearerToken}"));
    }
}
