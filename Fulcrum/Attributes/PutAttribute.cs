using Fulcrum.Models;

namespace Fulcrum
{
    public sealed class PutAttribute : MethodAttribute
    {
        public PutAttribute(string route)
            : base(route, HttpRequestMethod.PUT)
        {
        }
    }
}
