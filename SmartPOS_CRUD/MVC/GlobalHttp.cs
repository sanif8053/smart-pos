using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MVC
{
    public static class GlobalHttp
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalHttp()
        {
            WebApiClient.BaseAddress = new Uri("http://localhost:54747/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}