using Fulcrum.Models;
using System.Reflection;

namespace Fulcrum
{
    public sealed class BodyAttribute : ParameterAttribute
    {
        public string ContentType { get; }

        public BodyAttribute()
            : this("application/json")
        {
        }

        public BodyAttribute(string contentType)
        {
            ContentType = contentType;
        }

        internal override void ConfigureParameter(ParameterConfig config, ParameterInfo parameter) => config.IsRequestBody = true;
    }
}
