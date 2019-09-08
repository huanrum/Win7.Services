using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public static class Extend
    {
        /// <summary>
        /// 转换成MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToMD5(this string input)
        {
            var result = new System.Text.StringBuilder();
            var  bs = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2").ToLower());
            }
            return result.ToString();
        }
    }
}
