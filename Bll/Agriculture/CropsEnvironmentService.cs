using OracleDal.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Agriculture
{
    public class CropsEnvironmentService
    {
        IDal.Agriculture.ICropsEnvironment dal = new CropsEnvironmentRepository();
        public IList<Model.Agriculture.Topic_CropsEnvironment> List(string town)
        {
            var list = dal.List(" town='" + town + "'");
            return list;
        }
    }
}
