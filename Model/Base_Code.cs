using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class Base_Code
    {

        /// <summary>
        /// 字典表构造函数
        /// </summary>
        public Base_Code()
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
        ///编码
        /// </summary>
        public string SCode
        {
            get;
            set;
        }

        /// <summary>
        ///编码名称
        /// </summary>
        public string SName
        {
            get;
            set;
        }

        /// <summary>
        ///域名称
        /// </summary>
        public string DomainName
        {
            get;
            set;
        }

        /// <summary>
        ///域中文名称
        /// </summary>
        public string DomainAliasName
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
        /// <summary>
        ///创建用户
        /// </summary>
        public string CreateUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
