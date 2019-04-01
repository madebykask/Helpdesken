using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DH.Helpdesk.Web.Infrastructure
{
    internal class UrlBuilder
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _qsParams = new Dictionary<string, string>();

        #region ctor()

        public UrlBuilder(string urlMask)
        {
            if (!string.IsNullOrEmpty(urlMask))
            {
                //do parsing
                var index = urlMask.IndexOf('?');
                if (index != -1)
                {
                    _baseUrl = urlMask.Substring(0, index);
                }

                var qs = urlMask.Substring(urlMask.IndexOf('?'));
                _qsParams = ParseQueryString(qs);
            }
        }

        #endregion

        #region Factory Method

        public static UrlBuilder Create(string urlMask)
        {
            return new UrlBuilder(urlMask);
        }

        #endregion

        public UrlBuilder SetParam(string key, string value)
        {
            if (!_qsParams.ContainsKey(key))
            {
                _qsParams.Add(key, value);
            }

            _qsParams[key] = value;

            return this;
        }

        public UrlBuilder RemoveParam(string key)
        {
            if (_qsParams.ContainsKey(key))
            {
                _qsParams.Remove(key);
            }

            return this;
        }

        public UrlBuilder ClearParams()
        {
            _qsParams?.Clear();
            return this;
        }

        public string BuildUrl()
        {
            var first = true;
            var strBld = new StringBuilder();
            strBld.Append(_baseUrl);
            strBld.AppendFormat("?");

            foreach (var kv in _qsParams)
            {
                if (!first)
                    strBld.Append("&");

                strBld.Append($"{kv.Key}={kv.Value}");
                first = false;
            }

            return strBld.ToString();
        }

        #region Helper Methods

        private Dictionary<string, string> ParseQueryString(string queryString)
        {
            const string expr = @"[\\?&]([^&=]+)=([^&=]+)?";
            var qsParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var regex = new Regex(expr);

            foreach (Match m in regex.Matches(queryString, 0))
            {
                if (m.Success && m.Groups.Count == 3)
                    qsParams.Add(m.Groups[1].Value, m.Groups[2].Value);
            }
            return qsParams;
        }

        #endregion
    }
}