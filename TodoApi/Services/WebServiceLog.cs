using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Services
{
    public class WebServiceLog : IWebServiceLog
    {
        private IConfiguration Configuration { get; }
        public string LoggerId { get; set; }
        public string Enviroment { get; set; }

        public WebServiceLog(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerId = Configuration.GetValue<string>("WebServiceLog:LoggerId");
            Enviroment = Configuration.GetValue<string>("WebServiceLog:Enviroment");
        }
        
        private void Log(string thread, int level, string logger, string method, string location, string message, string exception)
        {
            throw new NotImplementedException();
        }

        private void Log(int level, string message, string exception, string method)
        {
            throw new NotImplementedException();
        }

        public void WebTraffic(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress;

            var request = context.Request;
            var method = request.Method;
            var path = request.Path.Value;
            var query = request.QueryString;
            var contentType = request.ContentType;
            var host = request.Host;
            var date = DateTime.Now.ToString("s");

            var response = context.Response;
            var statusCode = response.StatusCode;
            var level = (int)Level.Information;

            var requestLogMessage = $"REQUEST:\n{request.Method} - {request.Path.Value}{request.QueryString}";
            requestLogMessage += $"\nContentType: {request.ContentType ?? "Not specified"}";
            requestLogMessage += $"\nHost: {request.Host}";
            requestLogMessage += $"\nLevel: {level}";
            File.AppendAllText("log.txt", $"{DateTime.Now.ToString("s")}\n{requestLogMessage}");

            var responseLogMessage = $"\nRESPONSE:\nStatus Code: {response.StatusCode}";
            File.AppendAllText("log.txt", $"{responseLogMessage}\n\n");
        }

        public void Api(Exception e)
        {
            
        }

        private void Logger()
        {
            var thread = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private enum Level
        {
            Trace = 0,
            Debug = 1,
            Information = 2,
            Warning = 3,
            Error = 4,
            Critical = 5
        }
    }
}
