using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fulcrum.Models
{
    internal class ParameterConfig
    {
        public string Name { get; }
        public string Alias { get; internal set; }

        internal bool IsRouteReplacement { get; set; }
        internal bool IsQueryParams { get; set; }
        internal bool IsRequestHeader { get; set; }
        internal bool IsRequestBody { get; set; }

        private IEnumerable<ParameterAttribute> _attributes { get; set; }

        public ParameterConfig(ParameterInfo parameter)
        {
            Name = parameter.Name;

            InspectParameter(parameter);
        }

        public string ReplaceRoute(string url, object value)
        {
            if (!IsRouteReplacement)
                return url;

            var replacementString = "{" + (string.IsNullOrWhiteSpace(Alias) ? Name : Alias) + "}";

            return url.Replace(replacementString, value?.ToString());
        }

        public HeaderCollection GetHeaders(object value)
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
