using System;
using System.Collections.Specialized;
using System.Web;

namespace SharePointKnowledgeBase.WebPartCode.Application
{
    /// <summary>
    /// Helper class to parse and extract values from a
    /// url with query strings in it.
    /// </summary>
    /// <example>
    /// <code>
    /// // create a query object using the current url
    /// MyQuery = new UrlQuery();
    /// // add another parameter (or replace existing one)
    /// MyQuery["myparam"] = "myval";
    /// Trace.Write(MyQuery["myparam"]); // returns 'myval'
    /// // remove parameter
    /// MyQuery["myparam"] = null; // or string.Empty
    /// Trace.Write(MyQuery["myparam"]); // returns ''
    /// </code>
    /// </example>
    /// <remarks>
    /// TODO move to BilSimser.SharePoint.Common library
    /// </remarks>
    public class UrlQuery
    {
        private NameValueCollection _queryString;
        private readonly string _url;

        public UrlQuery(string value)
        {
            var q = value.IndexOf('?');
            if (q != -1)
            {
                _url = value.Substring(0, q);
                _queryString = NameValueCollection(value);
            }
            else
            {
                _url = value;
            }
        }

        public UrlQuery()
        {
            _url = HttpContext.Current.Request.Url.AbsolutePath;
        }

        public NameValueCollection QueryString
        {
            get
            {
                if (_queryString != null)
                {
                    return _queryString;
                }
                _queryString = new NameValueCollection(HttpContext.Current.Request.QueryString);
                return _queryString;
            }
        }

        public string AbsoluteUri
        {
            get { return Url + Get(); }
        }

        public string VirtualFolder
        {
            get { return Url.Substring(0, Url.LastIndexOf("/") + 1); }
        }

        public string Url
        {
            get { return _url; }
        }

        public string this[string param]
        {
            get { return Get(param); }
            set { Set(param, value); }
        }

        public void Set(string param, string value)
        {
            if (param == string.Empty) return;
            
            if (string.IsNullOrEmpty(value))
            {
                QueryString.Remove(param);
            }
            else
            {
                QueryString[param] = value;
            }
        }

        public static NameValueCollection NameValueCollection(string qs)
        {
            var nvc = new NameValueCollection();
            qs = qs.IndexOf('?') > 0 ? qs.Remove(0, qs.IndexOf('?') + 1) : qs;
            Array sqarr = qs.Split("&".ToCharArray());
            for (var i = 0; i < sqarr.Length; i++)
            {
                var pairs = sqarr.GetValue(i).ToString().Split("=".ToCharArray());
                nvc.Add(pairs[0], pairs[1]);
            }
            return nvc;
        }

        public void FormToQuery(string param)
        {
            Set(param, HttpContext.Current.Request.Form[param]);
        }

        public string Get(string param)
        {
            return QueryString[param];
        }

        public string Get()
        {
            var query = "";
            if (QueryString.Count != 0)
            {
                query = "?";
                for (var i = 0; i <= QueryString.Count - 1; i++)
                {
                    if (i != 0)
                    {
                        query += "&";
                    }
                    query += QueryString.GetKey(i) + "=" + QueryString.Get(i);
                }
            }
            return query;
        }
    }
}