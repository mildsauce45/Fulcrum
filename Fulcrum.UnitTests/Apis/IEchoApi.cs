using Fulcrum.UnitTests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests.Apis
{    
    public interface IEchoApi
    {
        [Get("public")]
        Task<Response> GetEcho([QueryParams] IDictionary<string, string> queryMap);

        [Get("public/{route}")]
        Task<Response> GetEchoRouteReplace(string route, [QueryParams] IDictionary<string, string> queryMap);

        [Get("public/{replace}/multiple/{route}")]
        Task<Response> GetReplaceMultipleRoutes(string replace, string route);

        [Post("private")]        
        Task<Response> Post([Body] Set set);

        [Post("private")]
        Task PostUntyped([Body] Set set);
    }
}
