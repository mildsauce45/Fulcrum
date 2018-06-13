using EchoService.Models;
using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace EchoService.Extensions
{
    public static class ModuleExtensions
    {
        public static void AddQueryParams(this NancyModule module, RequestBase request)
        {
            var keys = module.Request.Query.Keys as IEnumerable<string>;
            if (keys.Any())
            {
                request.QueryParams = new Dictionary<string, string>();
                foreach (var k in keys)
                    request.QueryParams[k] = module.Request.Query[k];
            }
        }

        public static void AddHeaders(this NancyModule module, RequestBase request)
        {
            var headers = module.Request.Headers as RequestHeaders;
            if (headers.Any())
            {
                request.Headers = new Dictionary<string, string>();
                foreach (var h in headers)
                    request.Headers.Add(h.Key, string.Join("; ", h.Value));
            }
        }
    }
}