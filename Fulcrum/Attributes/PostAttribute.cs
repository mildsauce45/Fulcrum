using Fulcrum.Models;

namespace Fulcrum
{
    public class PostAttribute : MethodAttribute
    {
        public PostAttribute(string route)
            : base(route, HttpRequestMethod.POST)
        {
        }
    }
}
