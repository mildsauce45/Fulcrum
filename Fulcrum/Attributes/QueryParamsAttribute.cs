using System;
using System.Reflection;
using Fulcrum.Models;

namespace Fulcrum
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueryParamsAttribute : ParameterAttribute
    {
        internal override void ConfigureParameter(ParameterConfig config, ParameterInfo parameter) =>
            config.IsQueryParams = true;
    }
}
