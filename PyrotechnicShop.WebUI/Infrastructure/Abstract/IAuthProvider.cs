﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyrotechnicShop.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool Authentificate(string username, string password);
    }
}
