using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class CookieUser
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public object UserId { get; set; }
        /// <summary>
        /// 登陆账户
        /// </summary>
        public object UserName { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public object UserPwd { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public object DeptId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public object RoleId { get; set; }
        /// <summary>
        /// 系统样式选择 0，普通用户；1，管理员；2，超级管理员；
        /// </summary>
        public object Theme { get; set; }
        /// <summary>
        ///用户类型；0，普通用户；1，管理员；2，超级管理员；
        /// </summary>
        public object IsAdmin { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public object DeptName { get; set; }

        public CookieUser(object userId, object userName, object userPwd, object deptId, object roleId, object theme)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.UserPwd = userPwd;
            this.DeptId = deptId;
            this.RoleId = roleId;
            this.Theme = theme;
        }
        public CookieUser()
        {
        }
    }
}
