using Fulcrum.Models;

namespace Fulcrum
{
    public class PutAttribute : MethodAttribute
    {
        public PutAttribute(string route)
            : base(route, HttpRequestMethod.PUT)
        {
        }
    }
}
