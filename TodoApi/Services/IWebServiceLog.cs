using System;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Services
{
    public interface IWebServiceLog
    {
        void Traffic(HttpContext context);
        void Exception(Exception e);
    }
}
