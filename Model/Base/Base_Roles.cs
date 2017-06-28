using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    public class Base_Roles
    {

        /// <summary>
        /// 系统角色表构造函数
        /// </summary>
        public Base_Roles()
        {
            ///Todo
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
        ///角色名称
        /// </summary>
        public string RolesName
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
        /// <summary>
        ///允许编辑 不允许：1，默认：0
        /// </summary>
        public int AllowEdit
        {
            get;
            set;
        }

        /// <summary>
        ///允许删除 不允许：1，默认：0
        /// </summary>
        public int AllowDelete
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
    }
}
