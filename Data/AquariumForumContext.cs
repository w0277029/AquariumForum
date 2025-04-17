using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AquariumForum.Models;

namespace AquariumForum.Data
{
    // Changed DbContext to IdentityDbContext
    public class AquariumForumContext : IdentityDbContext<ApplicationUser>
    {
        public AquariumForumContext (DbContextOptions<AquariumForumContext> options)
            : base(options)
        {
        }

        public DbSet<AquariumForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<AquariumForum.Models.Comment> Comment { get; set; } = default!;
    }
}
