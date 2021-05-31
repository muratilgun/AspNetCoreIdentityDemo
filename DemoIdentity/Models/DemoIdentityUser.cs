﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoIdentity.Models
{
    public class DemoIdentityUser : IdentityUser
    {
        public string Locale { get; set; } = "en-GB";
        public string OrgId { get; set; }
    }

    public class Organization
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
