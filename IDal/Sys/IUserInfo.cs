﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDal.Sys
{
    public interface IUserInfo : IBaseRepository<Model.Base_UserInfo>
    {
        Model.Base_UserInfo CheckUserInfo(string _UserId, string _UserPwd);
    }
}
