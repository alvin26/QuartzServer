using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UserJob;

namespace Job1
{
    public class ExecJob : IUserJob
    {
        public void Execute(NameValueCollection Parameters)
        {
            foreach (var item in Parameters.AllKeys)
            {
                Console.WriteLine("Key={0}, value={1}", item, Parameters[item]);
            }
        }
    }
}
