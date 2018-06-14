using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fulcrum.Models
{
    internal class ParameterConfig
    {
        private IEnumerable<ParameterAttribute> _attributes;

        internal string Name { get; }
        internal string Alias { get; set; }

        internal bool IsRouteReplacement { get; set; }
        internal bool IsQueryParams { get; set; }
        internal bool IsRequestHeader { get; set; }
        internal bool IsRequestBody { get; set; }        

        internal ParameterConfig(ParameterInfo parameter)
        {
            Name = parameter.Name;

            InspectParameter(parameter);
        }

        internal string ReplaceRoute(string url, object value)
        {
            if (!IsRouteReplacement)
                return url;

            var replacementString = "{" + (string.IsNullOrWhiteSpace(Alias) ? Name : Alias) + "}";

            return url.Replace(replacementString, value?.ToString());
        }

        internal HeaderCollection GetHeaders(object value)
        {
            if (!IsRequestHeader)
                return null;

            return new HeaderCollection(_attributes.OfType<HeaderAttribute>().Select(ha => ha.GetHeader(value)));
        }

        private void InspectParameter(ParameterInfo pi)
        {
            _attributes = pi.GetCustomAttributes<ParameterAttribute>();
            if (!_attributes.Any())
                IsRouteReplacement = true;
            else
            {
                foreach (var attr in _attributes)
                    attr.ConfigureParameter(this, pi);
            }
        }
    }
}
