using System;
using System.Threading.Tasks;

namespace Fulcrum
{
    public interface IAuthenticationProvider
    {
        Task<Tuple<string, string>> GetAuthorizationHeader();
    }
}
