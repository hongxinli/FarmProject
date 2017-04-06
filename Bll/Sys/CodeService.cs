using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleDal.Sys;
using IDal.Sys;
using System.Data;

namespace Bll.Sys
{
    public class CodeService
    {
        ICode DAL = new CodeRepository();
        /// <summary>
        ///  获取集合
        /// </summary>
        /// <param name="domainName">域名称</param>
        /// <returns></returns>
        public IList<Model.Base_Code> GetCode(string domainName)
        {
            return DAL.List(" domainname ='" + domainName + "'");
        }
    }
}
