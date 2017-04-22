using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    /// <summary>
    /// 专题图管理信息表
    /// </summary>
    public class A_Caption
    {

        /// <summary>
        /// 专题图管理信息表构造函数
        /// </summary>
        public A_Caption()
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
        ///专题图名称
        /// </summary>
        public string CaptionName
        {
            get;
            set;
        }

        /// <summary>
        ///专题图路径
        /// </summary>
        public string CaptionUrl
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
