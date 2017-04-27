using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDal.Agriculture
{
    public interface ICrop : IBaseRepository<Model.Agriculture.A_Crop>
    {
        int AddModel(Model.Agriculture.A_Crop model);
        int UpateModel(Model.Agriculture.A_Crop model);
    }
}
