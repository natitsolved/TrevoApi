using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trevo.API.Utility
{
    public class WebAppHelper
    {
        /// <summary>
        /// Gets Application Host Url
        /// </summary>
        /// <returns></returns>
        public static string GetHostUrl()
        {
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
            return !string.IsNullOrWhiteSpace(url) ? url : string.Empty;
        }
    }
}