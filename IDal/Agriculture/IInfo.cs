using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDal.Agriculture
{
    public interface IInfo : IBaseRepository<Model.Agriculture.A_Info>
    {
        int AddModel(Model.Agriculture.A_Info model);
        int UpateModel(Model.Agriculture.A_Info model);
    }
}
