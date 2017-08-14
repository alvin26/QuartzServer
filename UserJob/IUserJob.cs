using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace UserJob
{
    public interface IUserJob
    {
        void Execute(NameValueCollection Parameters);
    }
}
