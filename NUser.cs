using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations; 

namespace MManatee.Models
{
    public class NUser 
    {
        public int NUserID { get; set; }
        public string name { get; set;}
        public List<NUser> notify_users { get; set;}

    }
}