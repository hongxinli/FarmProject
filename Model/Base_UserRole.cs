using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户角色对应信息表
    /// </summary>
    public class Base_UserRole
    {

        /// <summary>
        /// 用户岗位对应信息构造函数
        /// </summary>
        public Base_UserRole()
        {
            ///Todo
        }
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
        ///角色编号
        /// </summary>
        public string RolesId
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
