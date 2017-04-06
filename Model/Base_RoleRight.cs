using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 系统角色模块关系表
    /// </summary>
    public class Base_RoleRight
    {

        /// <summary>
        /// 系统角色模块关系表构造函数
        /// </summary>
        public Base_RoleRight()
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
