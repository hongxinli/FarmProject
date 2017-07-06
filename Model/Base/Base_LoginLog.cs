using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户日志表
    /// </summary>
    public class Base_LoginLog
    {

        /// <summary>
        /// 用户日志表构造函数
        /// </summary>
        public Base_LoginLog()
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
        ///用户编号
        /// </summary>
        public string UserId
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
        ///ip地址
        /// </summary>
        public string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        ///在线状态 是否在线  1:在线   0:已下线
        /// </summary>
        public int IsOnline
        {
            get;
            set;
        }

        /// <summary>
        ///登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;
            set;
        }

        /// <summary>
        ///退出时间
        /// </summary>
        public DateTime ExitTime
        {
            get;
            set;
        }

        /// <summary>
        ///单位标识
        /// </summary>
        public string Deptid
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
