using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Dto
{
    public class jsonData<T> where T : class ,new()
    {
        public bool status { get; set; }
        public List<T> details { get; set; }
    }
}