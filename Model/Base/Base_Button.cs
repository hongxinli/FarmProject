using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 权限按钮操作表
    /// </summary>
    public class Base_Button
    {

        /// <summary>
        /// 权限按钮操作表构造函数
        /// </summary>
        public Base_Button()
        {
            ///Todo
        }

        /// <summary>
        ///按钮编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        ///按钮名称
        /// </summary>
        public string ButtonName
        {
            get;
            set;
        }

        /// <summary>
        ///按钮标记
        /// </summary>
        public string ButtonTitle
        {
            get;
            set;
        }

        /// <summary>
        ///按钮图标
        /// </summary>
        public string ButtonImg
        {
            get;
            set;
        }

        /// <summary>
        ///方法名称
        /// </summary>
        public string ButtonCode
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
        ///类型
        /// </summary>
        public string ButtonType
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
        ///删除标记 删除：1，默认：0
        /// </summary>
        public int DeleteMark
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
        ///创建世间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
