using Fulcrum.Models;

namespace Fulcrum
{
    public class DeleteAttribute : MethodAttribute
    {
        public DeleteAttribute(string route)
            : base(route, HttpRequestMethod.DELETE)
        {
        }
    }
}
