using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 功能模块表
    /// </summary>
    public class Base_ModuleInfo
    {

        /// <summary>
        /// 功能模块表构造函数
        /// </summary>
        public Base_ModuleInfo()
        {
            ///Todo
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
        ///父级标识
        /// </summary>
        public string ParentId
        {
            get;
            set;
        }

        /// <summary>
        ///模块名称
        /// </summary>
        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        ///模块标记
        /// </summary>
        public string ModuleTitle
        {
            get;
            set;
        }

        /// <summary>
        ///模块图标
        /// </summary>
        public string ModuleImg
        {
            get;
            set;
        }

        /// <summary>
        ///类型
        /// </summary>
        public int ModuleType
        {
            get;
            set;
        }

        /// <summary>
        ///导航地址 功能指向页面或反射实体类名
        /// </summary>
        public string NavigateUrl
        {
            get;
            set;
        }

        /// <summary>
        ///目标
        /// </summary>
        public string Target
        {
            get;
            set;
        }

        /// <summary>
        ///排序
        /// </summary>
        public int SortCode
        {
            get;
            set;
        }

        /// <summary>
        ///创建人
        /// </summary>
        public string Creator
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
        ///删除标记 删除：1，默认：0
        /// </summary>
        public int DeleteMark
        {
            get;
            set;
        }

        /// <summary>
        ///允许编辑 允许：1，默认：0
        /// </summary>
        public int AllowEdit
        {
            get;
            set;
        }

        /// <summary>
        ///允许删除 允许：1，默认：0
        /// </summary>
        public int AllowDelete
        {
            get;
            set;
        }
    }
}
