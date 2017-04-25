using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tincher.Interop
{
    /// <summary>
    /// 注册信息
    /// </summary>
    public class Ticket
    {
        /// <summary>
        ///  客户名称
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public string DueDate { get; set; }
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public string RegDate { get; set; }
        /// <summary>
        /// 试用版
        /// </summary>
        public bool Trial { get; set; }
        /// <summary>
        /// 用户数限制
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 验证通过
        /// </summary>
        public bool Allow { get; set; }
    }
}
