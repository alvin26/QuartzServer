using Common.Logging;
using ExecuteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Remoting;
using UserJob;

namespace JobProxy
{
    [Serializable]
    public class Proxy : System.MarshalByRefObject, IExecuteProxy
    {
        private readonly ILog logger;

        public Proxy()
        {
            logger = LogManager.GetLogger(GetType());
        }

        public void Execute(string DLLName, string TypeName, NameValueCollection Parameters)
        {

            string JobName = Parameters["JobName"];
            try
            {
                //string TypeName = Parameters["TypeName"];
                //string DLLName = Parameters["DLLName"];
                ObjectHandle handle = Activator.CreateInstance(DLLName, TypeName);
                object instance = handle.Unwrap();
                Type type = instance.GetType();

                //把屬性設定值寫入job instance
                if (Parameters != null)
                {
                    foreach (var item in Parameters.AllKeys)
                    {
                        PropertyInfo propInfo = type.GetProperty(item);
                        propInfo.SetValue(instance, Parameters[item], null);
                    }
                }

                //使用 IUserJob                
                IUserJob job = instance as IUserJob;
                job.Execute(Parameters);


            }
            catch (Exception ex)
            {
                logger.Error(string.Format("JobProxy.Proxy Execute 失敗:DLL Name:{0}, JobName:{1}, Error:{2}\n{3}", DLLName, JobName, ex.Message, ex.InnerException));
            }
        }


    }
}
