using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Dto
{
    public class pageData<T> where T : class ,new()
    {
        public int totalRow { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public IList<T> list { get; set; }
    }

    public class jsonModelData<T> where T : class ,new()
    {
        public bool status { get; set; }
        public T details { get; set; }
    }
}