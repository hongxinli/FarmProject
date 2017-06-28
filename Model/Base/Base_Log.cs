using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 系统日志表
    /// </summary>
    public class Base_Log
    {

        /// <summary>
        /// 系统日志表构造函数
        /// </summary>
        public Base_Log()
        {
            ///Todo
        }

        /// <summary>
        ///编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        ///用户名称
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        ///用户IP
        /// </summary>
        public string UserIp
        {
            get;
            set;
        }

        /// <summary>
        ///操作时间
        /// </summary>
        public DateTime OperationTime
        {
            get;
            set;
        }

        /// <summary>
        ///操作内容
        /// </summary>
        public string Operation
        {
            get;
            set;
        }

        /// <summary>
        ///单位标识
        /// </summary>
        public string DeptId
        {
            get;
            set;
        }

        /// <summary>
        ///功能标识
        /// </summary>
        public string MenuId
        {
            get;
            set;
        }
    }
}
