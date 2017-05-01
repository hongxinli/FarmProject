using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDal.Agriculture
{
    public interface IPest : IBaseRepository<Model.Agriculture.A_Pest>
    {
        int AddModel(Model.Agriculture.A_Pest model);
        int UpateModel(Model.Agriculture.A_Pest model);
    }
}
