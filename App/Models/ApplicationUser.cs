﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace App.Models {
    public class ApplicationUser : IdentityUser, IUser {
    }
}
