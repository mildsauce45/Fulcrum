using Fulcrum.Models;

namespace Fulcrum
{
    public sealed class PostAttribute : MethodAttribute
    {
        public PostAttribute(string route)
            : base(route, HttpRequestMethod.POST)
        {
        }
    }
}
