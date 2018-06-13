using Nancy.Security;
using System.Collections.Generic;

namespace EchoService.Auth
{
    public class UserIdentity : IUserIdentity
    {
        public string UserName => "TestUser";

        public IEnumerable<string> Claims => new string[] { };
    }
}