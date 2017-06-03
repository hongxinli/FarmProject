using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Agriculture
{
    public class Topic_CropsEnvironment
    {
        public string town { get; set; }
        public string village { get; set; }

        public string rice { get; set; }
        public string wheat { get; set; }
        public string cotton { get; set; }
        public string rape { get; set; }
        public string landtype { get; set; }
        public string soiltype { get; set; }
        public decimal area { get; set; }
    }
}
