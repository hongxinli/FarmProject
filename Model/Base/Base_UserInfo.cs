using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class Base_UserInfo
    {

        /// <summary>
        /// 用户信息表构造函数
        /// </summary>
        public Base_UserInfo()
        {
            ///Todo
        }

        /// <summary>
        ///用户帐号
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
        ///登录密码
        /// </summary>
        public string UserPwd
        {
            get;
            set;
        }

        /// <summary>
        ///联系电话
        /// </summary>
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        ///电子邮件
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        ///用户类型 用户类型；0，普通用户；1，管理员；2，超级管理员；
        /// </summary>
        public int IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        ///用户状态 0:启用，1是禁用；用户是否禁用，默认为启用；
        /// </summary>
        public int IsState
        {
            get;
            set;
        }

        /// <summary>
        ///录入人
        /// </summary>
        public string Creator
        {
            get;
            set;
        }

        /// <summary>
        ///录入时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        ///备注说明
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }
        /// <summary>
        /// 系统样式选择  0  超级管理员 1普通用户
        /// </summary>
        public string Theme { get; set; }
        /// <summary>
        /// AM账号
        /// </summary>
        public string Am { get; set; }
    }
}
