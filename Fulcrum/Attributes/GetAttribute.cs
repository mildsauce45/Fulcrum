using Fulcrum.Models;

namespace Fulcrum
{
    public sealed class GetAttribute : MethodAttribute
    {
        public GetAttribute(string route)
            : base(route, HttpRequestMethod.GET)
        {
        }
    }
}
