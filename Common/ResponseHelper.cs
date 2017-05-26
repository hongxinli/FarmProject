using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
    public static class ResponseHelper
    {
        public static void Write(string json)
        {

            HttpContext.Current.Response.Charset = "GB2312"; //设置字符集类型  
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.Write(json);
            HttpContext.Current.Response.End();
        }
    }
}
