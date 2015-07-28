using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MManatee.Models
{
    public class MManateeEntities : DbContext
    {
        public DbSet<Message> MessageSet { get; set; }
        public DbSet<Type> TypeSet { get; set; }
        // public DbSet<NUser> NUserSet { get; set; }
    }
}