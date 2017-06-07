using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Dto
{
    public class ChartData
    {
        public List<string> labels { get; set; }

        public List<BarSets> datasets { get; set; }
    }
    public class BarSets
    {
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public List<decimal> data { get; set; }
    }
}