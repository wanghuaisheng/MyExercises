using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using Exceptionless.Models;
using NLog;
using NLog.Fluent;
using NLog.Targets;

namespace NLogLearn
{
    class Program
    {
        private static readonly Logger Loger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            ExceptionlessTest();


            //Target.Register<MyCustomTarget>("MyCustom"); //generic

            Loger.Trace("trace message");
            Loger.Debug("debug message");
            Loger.Info("info message");
            Loger.Warn("warn message");
            Loger.Error("error message");
            Loger.Fatal("fatal message");
            Loger.Log(LogLevel.Debug, "debug message2");
            Loger.Log(LogLevel.Info, "info message {0} {1}", "para1", "para2");


            Console.ReadLine();

        }

        /// <summary>
        /// 测试Exceptionless提交
        /// </summary>
        private static void ExceptionlessTest()
        {
            ExceptionlessClient.Default.Startup("pKtE3wvLmB0fhC5fE86BCP7jtrxmWjPulc7HBo4O");


            // 发送日志
            ExceptionlessClient.Default.SubmitLog("Logging made easy");

            // 你可以指定日志来源，和日志级别。
            // 日志级别有这几种: Trace, Debug, Info, Warn, Error
            ExceptionlessClient.Default.SubmitLog(typeof(Program).FullName, "This is so easy", "Info");
            ExceptionlessClient.Default.CreateLog(typeof(Program).FullName, "This is so easy", "Info").AddTags("Exceptionless")
                .Submit();

            // 发送 Feature Usages
            ExceptionlessClient.Default.SubmitFeatureUsage("MyFeature");
            ExceptionlessClient.Default.CreateFeatureUsage("MyFeature").AddTags("Exceptionless").Submit();

            // 发送一个 404
            ExceptionlessClient.Default.SubmitNotFound("/somepage");
            ExceptionlessClient.Default.CreateNotFound("/somepage").AddTags("Exceptionless").Submit();

            // 发生一个自定义事件
            ExceptionlessClient.Default.SubmitEvent(new Event { Message = "Low Fuel", Type = "racecar", Source = "Fuel System" });


            new ArgumentException("测试参数异常").ToExceptionless().Submit();


            try
            {
                throw new ApplicationException(Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }

            var order = new { name = "name" };
            var user = new { Id = "Id", FullName = "FullName", EmailAddress = "Email" };

            try
            {
                throw new ApplicationException("Unable to create order from quote.");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless()
                    // 为事件设定一个编号，以便于你搜索 
                    .SetReferenceId(Guid.NewGuid().ToString("N"))
                    // 添加一个不包含CreditCardNumber属性的对象信息
                    .AddObject(order, "Order", excludedPropertyNames: new[] { "CreditCardNumber" }, maxDepth: 2)
                    // 设置一个名为"Quote"的编号
                    .SetProperty("Quote", 123)
                    // 添加一个名为“Order”的标签
                    .AddTags("Order")
                    // 标记为关键异常
                    .MarkAsCritical()
                    // 设置一个地理位置坐标
                    .SetGeo(43.595089, -88.444602)
                    // 设置触发异常的用户信息
                    .SetUserIdentity(user.Id, user.FullName)
                    // 设置触发用户的一些描述
                    .SetUserDescription(user.EmailAddress, "I tried creating an order from my saved quote.")
                    // 发送事件
                    .Submit();
            }


            //统一处理异常
            ExceptionlessClient.Default.SubmittingEvent += OnSubmittingEvent;
            //配合使用 NLog 或 Log4Net

            //有时候，程序中需要对日志信息做非常详细的记录，比如在开发阶段。这个时候可以配合 log4net 或者 nlog 来联合使用 exceptionless，详细可以查看这个 示例。

            //如果你的程序中有在短时间内生成大量日志的情况，比如一分钟产生上千的日志。这个时候你需要使用内存存储（in-memory store）事件，这样客户端就不会将事件系列化的磁盘，所以会快很多。这样就可以使用Log4net 或者 Nlog来将一些事件存储到磁盘，另外 Exceptionless 事件存储到内存当中。

            //using Exceptionless;
            //ExceptionlessClient.Default.Configuration.UseInMemoryStorage();
        }

        private static void OnSubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            // 仅处理未被处理过的异常
            if (!e.IsUnhandledError)
                return;

            // 忽略404事件
            if (e.Event.IsNotFound())
            {
                e.Cancel = true;
                return;
            }

            // 获取error对象
            var error = e.Event.GetError();
            if (error == null)
                return;

            // 忽略 401 或 `HttpRequestValidationException`异常
            if (error.Code == "401" || error.Type == "System.Web.HttpRequestValidationException")
            {
                e.Cancel = true;
                return;
            }

            // 忽略不是指定命名空间代码抛出的异常
            var handledNamespaces = new List<string> { "Exceptionless" };
            if (!error.StackTrace.Select(s => s.DeclaringNamespace).Distinct().Any(ns => handledNamespaces.Any(ns.Contains)))
            {
                e.Cancel = true;
                return;
            }

            e.Event.AddObject(order, "Order", excludedPropertyNames: new[] { "CreditCardNumber" }, maxDepth: 2);
            e.Event.Tags.Add("Order");
            e.Event.MarkAsCritical();
            e.Event.SetUserIdentity(user.EmailAddress);
        }

    }
}
