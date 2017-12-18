using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class WebServiceLog : IWebServiceLog
    {
        private IConfiguration Configuration { get; }
        public string ConnectionString { get; set; }
        public string LoggerId { get; set; }
        public string Enviroment { get; set; }

        public WebServiceLog(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerId = Configuration.GetValue<string>("WebServiceLog:LoggerId");
            Enviroment = Configuration.GetValue<string>("WebServiceLog:Enviroment");
        }

        void IWebServiceLog.Traffic(HttpContext context)
        {
            var method = context.Request.Method;
            var url = context.Request.Host + "/" + context.Request.Path.Value + context.Request.Query;
            var contentType = context.Request.ContentType;
            var statusCode = context.Response.StatusCode;
            var level = GetLevel(statusCode);
            var ipAddress = context.Connection.RemoteIpAddress;

            Logger(method, level.ToString(), "", "", url, contentType, statusCode.ToString(), ipAddress.ToString());
        }

        void IWebServiceLog.Exception(Exception e)
        {
            var method = GetMethod();
            const Enums.Level level = Enums.Level.Error;
            var message = e.Message;
            var exception = e.ToString();

            Logger(method, level.ToString(), message, exception, "", "", "", "");
        }

        private void Logger(string method, string level, string message, string exception, string url, string contentType, string statusCode, string ipAddress)
        {
            var thread = Thread.CurrentThread.ManagedThreadId.ToString();
            var loggerId = LoggerId;
            var userId = "userId";
            var publisherId = "publisherId";
            var enviroment = Enviroment;

            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    using (var cmd = new SqlCommand("dbo.InsLogs"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@publisherId", publisherId);
                        cmd.Parameters.AddWithValue("@thread", thread);
                        cmd.Parameters.AddWithValue("@loggerId", loggerId);
                        cmd.Parameters.AddWithValue("@enviroment", enviroment);
                        cmd.Parameters.AddWithValue("@method", method);
                        cmd.Parameters.AddWithValue("@level", level);
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters.AddWithValue("@exception", exception);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@conentType", contentType);
                        cmd.Parameters.AddWithValue("@statusCode", statusCode);
                        cmd.Parameters.AddWithValue("@ipAddress", ipAddress);

                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private static Enums.Level GetLevel(int statusCode)
        {
            if (statusCode < 400)
                return Enums.Level.Information;

            return statusCode < 500 ? Enums.Level.Error : Enums.Level.Critical;
        }

        private static string GetMethod()
        {
            // Todo: get correct method
            var stackTrace = new StackTrace();
            var methodBase = stackTrace.GetFrame(1).GetMethod();

            return "Incorrect!: " + methodBase.ToString();
        }
    }
}
