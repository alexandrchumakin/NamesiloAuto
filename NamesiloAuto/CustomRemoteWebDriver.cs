using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Remote;
using System.Web.Helpers;

namespace NamesiloAuto
{
    public class CustomRemoteWebDriver : RemoteWebDriver
    {
        public static bool newSession;
        public static string capPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "tmp", "sessionCap");
        public static string sessiodIdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "tmp", "sessionid");
        private object JsonConvert;

        public CustomRemoteWebDriver(Uri remoteAddress)
            : base(remoteAddress, new DesiredCapabilities())
        {
        }

        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            if (driverCommandToExecute == DriverCommand.NewSession)
            {
                if (!newSession)
                {
                    var capText = File.ReadAllText(capPath);
                    var sidText = File.ReadAllText(sessiodIdPath);

                    var cap = Json.Decode<Dictionary<string, object>>(capText);
                    return new Response
                    {
                        SessionId = sidText,
                        Value = cap
                    };
                }
                else
                {
                    var response = base.Execute(driverCommandToExecute, parameters);
                    var dictionary = (Dictionary<string, object>)response.Value;
                    File.WriteAllText(capPath, Json.Encode(dictionary));
                    File.WriteAllText(sessiodIdPath, response.SessionId);
                    return response;
                }
            }
            else
            {
                var response = base.Execute(driverCommandToExecute, parameters);
                return response;
            }
        }
    }
}
