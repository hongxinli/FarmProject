using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 单位表
    /// </summary>
    public class Base_Department
    {

        /// <summary>
        /// 单位表构造函数
        /// </summary>
        public Base_Department()
        {
            ///Todo
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
        ///父节点主键
        /// </summary>
        public string ParentId
        {
            get;
            set;
        }

        /// <summary>
        ///单位名称
        /// </summary>
        public string DeptName
        {
            get;
            set;
        }

        /// <summary>
        ///单位级别
        /// </summary>
        public int Dlevel
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
        ///备注说明
        /// </summary>
        public string Remarks
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
    }
}
