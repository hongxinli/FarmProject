using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    /// <summary>
    /// 农作物知识库管理表
    /// </summary>
    public class A_Crop
    {

        /// <summary>
        /// 农作物知识库管理表构造函数
        /// </summary>
        public A_Crop()
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
        ///作物名称
        /// </summary>
        public string CropName
        {
            get;
            set;
        }

        /// <summary>
        ///作物详细介绍
        /// </summary>
        public string CropContent
        {
            get;
            set;
        }

        /// <summary>
        ///作物分类
        /// </summary>
        public string CropType
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
