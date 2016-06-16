using Pandorum.Core.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    internal sealed class QueryBuilder
    {
        private readonly List<KeyValuePair<string, string>> _parameters;

        public QueryBuilder()
        {
            _parameters = new List<KeyValuePair<string, string>>();
        }

        // Needed to support C# 6 collection initializers
        public void Add(string key, string value)
        {
            _parameters.Add(key, value);
        }

        public string this[string key]
        {
            get
            {
                return _parameters.First(p => p.Key == key).Value;
            }
            set
            {
                int index = _parameters.FindIndex(p => p.Key == key);
                if (index != -1) // not found
                    _parameters[index] = new KeyValuePair<string, string>(key, value);
                else
                    throw new KeyNotFoundException();
            }
        }

        public override string ToString() =>
            ToString(appendQuestionMark: true, urlEncode: true);

        public string ToString(bool appendQuestionMark, bool urlEncode)
        {
            using (var lease = StringBuilderPool.Default.Lease())
            {
                var sb = lease.Item;

                bool firstIteration = true;
                foreach (var pair in _parameters)
                {
                    if (firstIteration)
                    {
                        if (appendQuestionMark) sb.Append('?');
                        firstIteration = false;
                    }
                    else sb.Append('&');

                    var key = pair.Key;
                    var value = pair.Value;
                    if (urlEncode) value = WebUtility.UrlEncode(value);

                    sb.Append(key).Append('=').Append(value);
                }

                return sb.ToString();
            }
        }
    }
}
