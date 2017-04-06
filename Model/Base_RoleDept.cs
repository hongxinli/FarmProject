using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 角色数据权限表
    /// </summary>
    public class Base_RoleDept
    {

        /// <summary>
        /// 角色数据权限表构造函数
        /// </summary>
        public Base_RoleDept()
        {
            ///Todo
        }
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        ///角色编号
        /// </summary>
        public string RolesId
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
        ///创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
    }

}
