using System;
using System.Collections.Generic;
using System.Linq;

namespace Fulcrum.Models
{
    internal class HeaderCollection
    {
        private IDictionary<string, IList<string>> _headers = new Dictionary<string, IList<string>>();

        public int Count => _headers.Count;

        public HeaderCollection()
        {
        }

        public HeaderCollection(IEnumerable<Tuple<string, string>> initialValues)
        {
            if (initialValues == null)
                return;

            foreach (var h in initialValues)
                Add(h.Item1, h.Item2);
        }

        public void Add(string header, string value)
        {
            // We'll do a case insensitive comparison of the header keys
            foreach (var k in _headers.Keys)
            {                
                if (string.Equals(k, header, StringComparison.OrdinalIgnoreCase))
                {
                    _headers[k].Add(value);
                    return;
                }
            }

            // If we got here, then we didn't locate the header in the existing collection so go ahead and add it now
            _headers.Add(header, new List<string>());
            _headers[header].Add(value);
        }

        public void Add(HeaderCollection collection)
        {
            var newHeaders = collection.GetHeaders();

            foreach (var header in newHeaders)
                Add(header.Item1, header.Item2);
        }

        public IEnumerable<Tuple<string, string>> GetHeaders()
        {
            return _headers.Select(kvp => Tuple.Create(kvp.Key, string.Join(";", kvp.Value)));
        }        
    }
}
