using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ExecuteInterface
{
    public interface IExecuteProxy
    {
        void Execute(string DLLName, string TypeName, NameValueCollection Parameters);
    }
}
