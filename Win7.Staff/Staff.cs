using System;
using System.Collections.Generic;
using System.Text;

namespace Win7.Staff
{
    public static class Staff
    {
        public static object To(this object sender,string code = "0",string message = null)
        {
            return new { returnCode = code, data = sender, errMessage = message };
        } 
    }
}
