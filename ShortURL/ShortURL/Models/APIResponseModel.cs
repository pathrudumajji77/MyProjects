using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ShortURL.Models
{
    public class APIResponseModel
    {
        public class ApiErrorMessage
        {
            public string result { get; set; }
            public string responsecode { get; set; }
            public string errorcause { get; set; }
            public string errordescription { get; set; }
        }
        public class ApiResponseMessage
        {
            public string result { get; set; }
            public string responsecode { get; set; }
        }

        public class ApiShortURLResponseMessage
        {
            public string Result { get; set; }
            public HttpStatusCode Responsecode { get; set; }
            public string ShortURL { get; set; }
            public string Errorcause { get; set; }
            public string Errordescription { get; set; }

        }

        public class ApiGetURLDetailsResponseMessage
        {
            public string Result { get; set; }
            public HttpStatusCode Responsecode { get; set; }
            public URLInfo URLInfo { get; set; }
            public string Errorcause { get; set; }
            public string Errordescription { get; set; }

        }
    }
}