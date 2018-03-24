using System.Diagnostics;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace NLogCustoms
{
    [Target("MyCustom")]
    public sealed class MyCustomTarget : TargetWithLayout
    {
        public MyCustomTarget()
        {
            Host = "localhost";
        }
        [RequiredParameter]
        public string Host { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);
            SendTheMessageToRemoteHost(Host, logMessage);
        }

        private void SendTheMessageToRemoteHost(string host, string message)
        {
            Debug.WriteLine($"{host}:{message}");
            // TODO - write me 
        }
    }
}
