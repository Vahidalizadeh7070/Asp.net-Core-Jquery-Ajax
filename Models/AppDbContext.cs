using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationUsingJqueryAjax.Models
{
    // AppDbContext
    // This class is going to use as the Context
    // This Context inherits from DbContext which is related to EF Core
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }

        // Contact DbSet
        public DbSet<Contact> Contacts { get; set; }

        // FeedBacks DbSet
        public DbSet<FeedBack> FeedBacks { get; set; }
    }
}
