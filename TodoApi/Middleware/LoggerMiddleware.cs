using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebServiceLog _logger;
        
        public LoggerMiddleware(RequestDelegate next, IWebServiceLog logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                
                _logger.WebTraffic(context);
            }
            catch (Exception e)
            {
                _logger.Api(e);
            }
        }
    }
}
