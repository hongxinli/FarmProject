using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    public class Topic_FarmLandLevel
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市总面积
        /// </summary>
        public string cityArea { get; set; }
        /// <summary>
        /// 城市等级面积
        /// </summary>
        public string cityLevelArea { get; set; }
        /// <summary>
        /// 乡镇
        /// </summary>
        public string town { get; set; }
        /// <summary>
        /// 乡镇总面积
        /// </summary>
        public string townArea { get; set; }
        /// <summary>
        /// 乡镇等级面积
        /// </summary>
        public string townLevelArea { get; set; }
        /// <summary>
        /// 村落
        /// </summary>
        public string village { get; set; }
        /// <summary>
        /// 村落总面积
        /// </summary>
        public string villageArea { get; set; }
        /// <summary>
        /// 村落等级面积
        /// </summary>
        public string villageLevelArea { get; set; }
    }
}
