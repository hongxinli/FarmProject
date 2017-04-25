using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Agriculture;
using OracleDal.Agriculture;

namespace Bll.Agriculture
{
    public class InfoService
    {
        IInfo dal = new InfoRepository();

    }
}
