using medprohiremvp.DATA.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Repo.Context
{
    public class AppUserContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppUserContext(DbContextOptions<AppUserContext> options)
            : base(options)
        {

        }
    }
    
}
