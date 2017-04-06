using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户单位关联表
    /// </summary>
    public class Base_UserDept
    {

        /// <summary>
        /// 用户单位关联表构造函数
        /// </summary>
        public Base_UserDept()
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
        ///单位编号
        /// </summary>
        public string DeptId
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
