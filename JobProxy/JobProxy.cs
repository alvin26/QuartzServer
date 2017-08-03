using Common.Logging;
using ExecuteInterface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace JobProxy
{
    [Serializable]
    public class JobProxy : System.MarshalByRefObject, IExecuteProxy
    {
        private readonly ILog logger;

        public JobProxy()
        {
            logger = LogManager.GetLogger(GetType());
        }

        public void Execute(NameValueCollection Parameters)
        {
             
        }
    }
}
