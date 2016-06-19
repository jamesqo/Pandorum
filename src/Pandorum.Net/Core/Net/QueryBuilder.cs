using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    // NOTE: This class is a mutable struct,
    // for the sake of avoiding allocations.
    internal struct QueryBuilder
    {
        private bool _first;
        private readonly StringBuilder _sb;

        public QueryBuilder(StringBuilder start)
        {
            _first = true;
            _sb = start;
        }

        public QueryBuilder Add(string key, string value)
        {
            if (value != null)
            {
                if (_first)
                {
                    _sb.Append('?');
                    _first = false;
                }
                else
                {
                    _sb.Append('&');
                }

                _sb.Append(key)
                    .Append('=')
                    .Append(WebUtility.UrlEncode(value));
            }
            return this;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
