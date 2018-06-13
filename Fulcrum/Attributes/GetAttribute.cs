using Fulcrum.Models;

namespace Fulcrum
{
    public class GetAttribute : MethodAttribute
    {
        public GetAttribute(string route)
            : base(route, HttpRequestMethod.GET)
        {
        }
    }
}
