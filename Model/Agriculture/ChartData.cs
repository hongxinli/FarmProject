using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    public class ChartData
    {
        public List<string> labels { get; set; }

        public List<BarSets> data { get; set; }
    }
    public class BarSets
    {
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public List<decimal> data { get; set; }
    }
}
