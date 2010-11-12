﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys
{
    public interface IAppMembershipProvider : IAppModelProvider
    {
        bool Authenticate(string username, string password);
    }
}
