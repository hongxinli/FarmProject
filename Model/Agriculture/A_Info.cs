using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    /// <summary>
    /// 农业信息管理表
    /// </summary>
    public class A_Info
    {

        /// <summary>
        /// 农业信息管理表构造函数
        /// </summary>
        public A_Info()
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
        ///信息标题
        /// </summary>
        public string InfoTitle
        {
            get;
            set;
        }

        /// <summary>
        ///信息内容
        /// </summary>
        public string InfoContent
        {
            get;
            set;
        }

        /// <summary>
        ///信息类型
        /// </summary>
        public string InfoType
        {
            get;
            set;
        }

        /// <summary>
        ///删除标志 删除：1，默认：0
        /// </summary>
        public string DeleteMark
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
        ///创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
