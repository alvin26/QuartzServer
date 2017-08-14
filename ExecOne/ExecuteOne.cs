using Common.Logging;
using ExecuteInterface;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Specialized;

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
            string domainName = string.Empty;
            string DLLName = string.Empty;
            string TypeName = string.Empty;
            try
            {
                NameValueCollection parameters = new NameValueCollection();
                foreach (var item in context.JobDetail.JobDataMap)
                {
                    parameters.Add(item.Key, string.Format("{0}", item.Value));
                }

                DLLName = parameters["_DLLName"];
                TypeName = parameters["_TypeName"];

                string cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache");
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;
                domaininfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                domaininfo.PrivateBinPath = "Jobs";
                //要使用ShadowCopyFiles 必須設定 ApplicationName 
                domaininfo.ApplicationName = string.Format("ApplicationName{0}", Thread.CurrentThread.Name);
                domaininfo.ShadowCopyFiles = "true";
                domaininfo.CachePath = cachePath; //暫存的位置


                System.Security.Policy.Evidence evi = AppDomain.CurrentDomain.Evidence;
                domainName = string.Format("Domain-{0}", Thread.CurrentThread.Name);
                AppDomain domain =
                    AppDomain.CreateDomain(
                        domainName,
                        evi,
                        domaininfo);

                //execute job
                IExecuteProxy proxy = domain.CreateInstanceFromAndUnwrap("JobProxy.dll", "JobProxy.Proxy") as IExecuteProxy;
                proxy.Execute(DLLName, TypeName, parameters);

                //Unload domain
                try
                {
                    AppDomain.Unload(domain);
                }
                catch (Exception exUnload)
                {
                    logger.Error(string.Format("Domain:{0} Unload 失敗:{1}\n{2}", exUnload.Message, exUnload.InnerException));
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Execute 失敗:{0}\n{1}", domainName, ex.Message, ex.InnerException));
            }
        }

        public void Interrupt()
        {
            throw new NotImplementedException();
        }
    }
}
