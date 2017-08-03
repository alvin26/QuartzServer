using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExecOne
{
    [DisallowConcurrentExecution]
    public class ExecuteOne : IInterruptableJob
    {
        private readonly ILog logger;
        public ExecuteOne()
        {
            logger = LogManager.GetLogger(GetType());
        }

        //關於 domain 的有用參考文件
        //https://msdn.microsoft.com/zh-tw/library/system.appdomainsetup.applicationname(v=vs.100).aspx
        //https://msdn.microsoft.com/zh-tw/library/system.loaderoptimizationattribute(v=vs.100).aspx
        //https://msdn.microsoft.com/zh-tw/library/system.loaderoptimization(v=vs.100).aspx
        //https://msdn.microsoft.com/zh-tw/library/ms404279(v=vs.100).aspx#EnablingAndUsing
        //https://msdn.microsoft.com/zh-tw/library/ms404279(v=vs.100).aspx
        //https://msdn.microsoft.com/zh-tw/library/system.appdomain.shadowcopyfiles(v=vs.100).aspx
        //https://msdn.microsoft.com/zh-tw/library/system.appdomain(v=vs.100).aspx
        //https://docs.microsoft.com/en-us/dotnet/framework/app-domains/how-to-configure-an-application-domain
        //https://docs.microsoft.com/en-us/dotnet/framework/app-domains/retrieve-setup-information
        //https://docs.microsoft.com/en-us/dotnet/framework/app-domains/use
        //https://msdn.microsoft.com/en-us/library/ms173140(v=vs.100).aspx


        public void Execute(IJobExecutionContext context)
        {

            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            domaininfo.PrivateBinPath = "Jobs";
            domaininfo.ShadowCopyFiles = "true";
            domaininfo.ApplicationName = string.Format("ApplicationName{0}", Thread.CurrentThread.Name);
            domaininfo.CachePath = "Cache";
            domaininfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;
            System.Security.Policy.Evidence evi = AppDomain.CurrentDomain.Evidence;
            AppDomain domain = AppDomain.CreateDomain(string.Format("Domain-{0}", Thread.CurrentThread.Name),
                , evi, domaininfo);
        }

        public void Interrupt()
        {
            throw new NotImplementedException();
        }
    }
}
