﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace LCL.MvcExtensions
{

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="HttpContextBase"/>.
    /// </summary>
    public static class HttpContextBaseExtensions
    {
        /// <summary>
        /// Compresses the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void Compress(this HttpContextBase instance)
        {

            HttpRequestBase httpRequest = instance.Request;

            // IE6
            if ((httpRequest.Browser.MajorVersion < 7) && httpRequest.Browser.IsBrowser("IE"))
            {
                return;
            }

            string preferedAcceptEncoding = httpRequest.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(preferedAcceptEncoding))
            {
                return;
            }

            HttpResponseBase httpResponse = instance.Response;

            Action<string> compress = encoding =>
                                          {
                                              if (!string.IsNullOrEmpty(httpResponse.RedirectLocation))
                                              {
                                                  return;
                                              }

                                              try
                                              {
                                                  httpResponse.AppendHeader("Content-encoding", encoding);
                                                  httpResponse.Filter = encoding.Equals("deflate") ? 
                                                                        new DeflateStream(httpResponse.Filter, CompressionMode.Compress) :
                                                                        new GZipStream(httpResponse.Filter, CompressionMode.Compress) as Stream;
                                              }
                                              catch (HttpException)
                                              {
                                              }
                                          };

            if (preferedAcceptEncoding.Equals("*", StringComparison.Ordinal) || preferedAcceptEncoding.Equals("gzip", StringComparison.OrdinalIgnoreCase))
            {
                compress("gzip");
            }
            else if (preferedAcceptEncoding.Equals("deflate", StringComparison.OrdinalIgnoreCase))
            {
                compress("deflate");
            }
        }

        /// <summary>
        /// Caches the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="duration">The duration.</param>
        public static void Cache(this HttpContextBase instance, TimeSpan duration)
        {

            HttpCachePolicyBase cache = instance.Response.Cache;

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetOmitVaryStar(true);
            cache.SetExpires(instance.Timestamp.Add(duration));
            cache.SetMaxAge(duration);
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(instance.Timestamp);
            cache.SetLastModifiedFromFileDependencies();
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        }
    }
}