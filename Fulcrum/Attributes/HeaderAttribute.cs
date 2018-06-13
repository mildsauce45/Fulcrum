using Fulcrum.Models;
using System;
using System.Reflection;

namespace Fulcrum
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
    public class HeaderAttribute : ParameterAttribute
    {
        public string Header { get; }

        private readonly string value;

        public HeaderAttribute(string header)
            : this(header, null)
        {            
        }

        /// <summary>
        /// Create a header with a static value.
        /// </summary>
        public HeaderAttribute(string header, string value)
        {
            Header = header;
            this.value = value;
        }

        internal override void ConfigureParameter(ParameterConfig config, ParameterInfo parameter) => config.IsRequestHeader = true;

        internal virtual Tuple<string, string> GetHeader(object liveValue) => Tuple.Create(Header, value ?? liveValue?.ToString());
    }
}
