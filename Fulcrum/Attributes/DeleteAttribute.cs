using Fulcrum.Models;

namespace Fulcrum
{
    public sealed class DeleteAttribute : MethodAttribute
    {
        public DeleteAttribute(string route)
            : base(route, HttpRequestMethod.DELETE)
        {
        }
    }
}
