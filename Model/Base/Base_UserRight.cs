using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户帐户模块关系表
    /// </summary>
    public class Base_UserRight
    {

        /// <summary>
        /// 用户帐户模块关系表构造函数
        /// </summary>
        public Base_UserRight()
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
        ///模块编号
        /// </summary>
        public string ModuleId
        {
            get;
            set;
        }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
