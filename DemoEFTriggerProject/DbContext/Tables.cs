using DemoEFTriggerProject.DbClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEFTriggerProject.DBContext
{
    public partial class DataContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<DbDemoClass> DbDemoClasses { get; set; }
    }
}
