using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Base_ModuleDept
    {
        /// <summary>
        /// 模块单位关联表构造函数
        /// </summary>
        public Base_ModuleDept()
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
        ///模块编号
        /// </summary>
        public string ModuleId
        {
            get;
            set;
        }

        /// <summary>
        ///单位编号
        /// </summary>
        public string DeptId
        {
            get;
            set;
        }
    }
}
