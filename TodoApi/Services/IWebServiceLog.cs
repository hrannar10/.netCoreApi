using System;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Services
{
    public interface IWebServiceLog
    {
        void WebTraffic(HttpContext context);
        void Api(Exception e);
    }
}
