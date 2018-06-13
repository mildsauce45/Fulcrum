using System;
using Fulcrum.Models;
using System.Reflection;

namespace Fulcrum
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class AliasAttribute : ParameterAttribute
    {
        public string Alias { get; }

        public AliasAttribute(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("alias cannot be null or whitespace");

            Alias = alias;
        }

        internal override void ConfigureParameter(ParameterConfig config, ParameterInfo parameter)
        {
            config.IsRouteReplacement = true;
            config.Alias = Alias;
        }
    }
}